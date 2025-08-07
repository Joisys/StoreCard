using AutoMapper;
using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Domain.Entities;

namespace StoreCard.Application.Profiles
{
    public class UserTransactionMappingProfile : Profile
    {
        public UserTransactionMappingProfile()
        {
            CreateMap<UserTransaction, UserTransactionDto>();
            CreateMap<UserTransactionCreateDto, UserTransaction>()
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate == default ? DateTime.UtcNow : src.TransactionDate));
        }
    }
}
