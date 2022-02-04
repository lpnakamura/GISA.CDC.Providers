using AutoMapper;
using GISA.ChangeDataCapture.Providers.Application.ViewModels;
using GISA.ChangeDataCapture.Providers.Domain.Entities;

namespace GISA.ChangeDataCapture.Providers.Application.Mappings
{
    public class ProviderMapping : Profile
    {
        public ProviderMapping()
        {
            CreateMap<Provider, ProviderResponseViewModel>().ReverseMap();
            CreateMap<ProviderRelationEntity, ProviderRelationViewModel>().ReverseMap();
            CreateMap<ProviderEntity, ProviderViewModel>().ReverseMap();
            CreateMap<SpecialityEntity, SpecialityViewModel>().ReverseMap();
            CreateMap<CityEntity, CityViewModel>().ReverseMap();

            CreateMap<Provider, ProvidersimplifiedResponseViewModel>().ReverseMap();
            CreateMap<ProviderRelationEntity, ProviderSimplifiedRelationViewModel>().ReverseMap();
            CreateMap<ProviderEntity, ProviderSimplifiedViewModel>().ReverseMap();
        }
    }
}
