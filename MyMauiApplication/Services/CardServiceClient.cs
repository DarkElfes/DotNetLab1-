using Library.IServices;
using System.Net.Http.Json;
using Library.Models.DTOs.Request;
using Library.Models.DTOs.Response;
using Library.Models.DTOs;

namespace MyMauiApplication.Services;

public class CardServiceClient(
    IHttpClientFactory httpClientFactory) : ICardServiceClient
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    public event Action<CardDTO>? OnCardChanged;
    public CardDTO? CardDTO { get; set; }


    public async Task<CardDTO?> GetCardAsync()
    {
        using HttpClient client = _httpClientFactory.CreateClient("ServerApi");
        CardDTO = await ResponseHandler<CardDTO>(await client.GetAsync("api/card"));
        OnCardChanged?.Invoke(CardDTO);
        return CardDTO;
    }

    public async Task<TransferResponse?> Transfer(TransferRequest transferRequest)
    {
        using HttpClient client = _httpClientFactory.CreateClient("ServerApi");
        return await ResponseHandler<TransferResponse>(await client.PutAsJsonAsync("api/card/transfer", transferRequest));
    }
    public async Task<TransferResponse?> Deposit(OperationRequest operationRequest)
    {
        using HttpClient client = _httpClientFactory.CreateClient("ServerApi");
        return await ResponseHandler<TransferResponse>(await client.PutAsJsonAsync("api/card/deposit", operationRequest));
    }

    public async Task<TransferResponse?> Withdraw(OperationRequest operationRequest)
    {
        using HttpClient client = _httpClientFactory.CreateClient("ServerApi");
        return await ResponseHandler<TransferResponse>(await client.PutAsJsonAsync("api/card/withdraw", operationRequest));
    }


    private async Task<T?> ResponseHandler<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        try
        {
            T obj = await response.Content.ReadFromJsonAsync<T>();

            if (typeof(T) == typeof(CardDTO))
                return obj;
            else if (obj is TransferResponse transferResponse)
            {
                CardDTO.Balance = transferResponse.CurrentBalance;
                CardDTO.TransactionDTOs.Add(transferResponse.TransactionDTO);
                OnCardChanged?.Invoke(CardDTO);
            }

            return obj;
        }
        catch
        {
            throw new InvalidDataException();
        }
    }
}
