using AutoMapper;
using Bank.Repository.IRepository;
using Bank.Services.IServices;
using Library.Models;
using Library.Models.DTOs;
using Library.Models.DTOs.Request;
using Library.Models.DTOs.Response;

namespace Bank.Services;

public class CardService(IHttpContextAccessor httpContextAccessor,
    IRepository<Transaction> transRepo,
    ICardRepository cardRepo,
    IUserRepository userRepo,
    IMapper mapper) : ICardService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IRepository<Transaction> _transRepo = transRepo;
    private readonly ICardRepository _cardRepo = cardRepo;
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IMapper _mapper = mapper;

    private Card GetCardFromContext()
        => _cardRepo.GetById(int.Parse(_httpContextAccessor.HttpContext.Items["card_id"].ToString()))!;

    private CardDTO GetCardDTO(Card card)
    {
        CardDTO cardDTO = _mapper.Map<CardDTO>(card);

        cardDTO.TransactionDTOs = ListFullMap(_transRepo.GetAll().Where(t => t.CardId == card.Id || t.ToCardId == card.Id).ToList());

        return cardDTO;
    }

    public CardDTO GetCard()
        => GetCardDTO(GetCardFromContext());

    public TransferResponse Transfer(TransferRequest transferRequest)
    {
        Card? userCard = GetCardFromContext();
        Card? toCard = _cardRepo.GetByNumber(transferRequest.To);


        Transaction transaction = new()
        {
            Type = TransactionType.Transfer,
            CardId = userCard.Id,
            Amount = transferRequest.Amount,
            CreateTime = DateTime.Now,
            Reason = transferRequest.Reason,
            IsSuccess = false,
            ToCardId = toCard?.Id ?? 0,
        };

        if (userCard is null || toCard is null)
            transaction.ErrorMessage = "Error, number card not exist";
        else if (userCard == toCard)
            transaction.ErrorMessage = "Error, you can't do a transfer of yourself";
        else if (userCard.Balance - transferRequest.Amount >= 0)
        {
            userCard.Balance -= transferRequest.Amount;
            toCard.Balance += transferRequest.Amount;

            transaction.IsSuccess = true;
            _cardRepo.Update(toCard);
        }
        else
            transaction.ErrorMessage = "Error, not have enough money";


        _transRepo.Create(transaction);
        _cardRepo.Update(userCard);

        return new()
        {
            CurrentBalance = userCard.Balance,
            TransactionDTO = FullMap(transaction),
        };
    }

    public TransferResponse Deposit(OperationRequest operationRequest)
    {
        Card? userCard = GetCardFromContext();


        Transaction transaction = new()
        {
            Type = TransactionType.Deposit,
            CardId = userCard.Id,
            Amount = operationRequest.Amount,
            CreateTime = DateTime.Now,
            IsSuccess = true,
        };

        userCard.Balance += operationRequest.Amount;

        _transRepo.Create(transaction);
        _cardRepo.Update(userCard);

        return new()
        {
            CurrentBalance = userCard.Balance,
            TransactionDTO = FullMap(transaction),
        };
    }

    public TransferResponse Withdraw(OperationRequest operationRequest)
    {
        Card? userCard = GetCardFromContext();

        Transaction transaction = new()
        {
            Type = TransactionType.Withdraw,
            CardId = userCard.Id,
            Amount = operationRequest.Amount,
            CreateTime = DateTime.Now,
            IsSuccess = false,
        };

        if (userCard.Balance > operationRequest.Amount)
        {
            userCard.Balance -= operationRequest.Amount;
            transaction.IsSuccess = true;
        }
        else
            transaction.ErrorMessage = "Error, not have enough money";

        _transRepo.Create(transaction);
        _cardRepo.Update(userCard);

        return new()
        {
            CurrentBalance = userCard.Balance,
            TransactionDTO = FullMap(transaction),
        };
    }

    private TransactionDTO FullMap(Transaction transaction)
    {
        TransactionDTO transactionDTO = _mapper.Map<TransactionDTO>(transaction);
        if (transaction.ToCardId != 0)
        {
            transactionDTO.CardUserName = _userRepo.GetByCardId(transaction.CardId)!.Name;
            transactionDTO.ToCardUserName = _userRepo.GetByCardId(transaction.ToCardId)!.Name;
        }
        return transactionDTO;
    }

    private List<TransactionDTO> ListFullMap(List<Transaction> transactions)
    {
        List<TransactionDTO> transactionDTOs = new();
        transactions.ForEach(t => transactionDTOs.Add(FullMap(t)));
        return transactionDTOs;
    }




}
