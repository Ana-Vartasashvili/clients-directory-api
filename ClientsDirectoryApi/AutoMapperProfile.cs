using AutoMapper;
using ClientsDirectoryApi.Clients;

namespace ClientsDirectoryApi;


public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Client, GetClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<Client, GetClientDto>();
        CreateMap<GetClientDto, Client>();
        CreateMap<UpdateClientDto, Client>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Account, GetAccountDto>();
        CreateMap<CreateAccountDto, Account>();
        CreateMap<GetFullClientDto, Client>();
        CreateMap<Client, GetFullClientDto>();
    }
}