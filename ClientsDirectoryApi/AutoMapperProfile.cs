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
    }
}