using AutoMapper;
using Bank.Repository.IRepository;
using Library.Models;
using Library.Models.DTOs;
using Library.Models.DTOs.Request;

namespace Bank.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterRequest>().ReverseMap();
        CreateMap<Card, CardDTO>();
        CreateMap<Transaction, TransactionDTO>();
    }
}
