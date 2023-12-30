using Library.Models.DTOs.Request;
using Library.Models.DTOs;
using Library.Models.DTOs.Response;

namespace Library.IServices;

public interface ICardServiceClient
{
    CardDTO CardDTO { get; set; }
    event Action<CardDTO>? OnCardChanged;
    Task<CardDTO?> GetCardAsync();

    Task<TransferResponse?> Transfer(TransferRequest transferRequest);
    Task<TransferResponse?> Withdraw(OperationRequest operationRequest);
    Task<TransferResponse?> Deposit(OperationRequest operationRequest);
}
