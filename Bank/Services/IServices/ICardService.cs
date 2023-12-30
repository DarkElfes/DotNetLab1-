using Library.Models.DTOs.Response;
using Library.Models.DTOs.Request;
using Library.Models.DTOs;

namespace Bank.Services.IServices;

public interface ICardService
{
    CardDTO GetCard();

    TransferResponse Transfer(TransferRequest transferRequest);
    TransferResponse Withdraw(OperationRequest operationRequest);
    TransferResponse Deposit(OperationRequest operationRequest);
}
