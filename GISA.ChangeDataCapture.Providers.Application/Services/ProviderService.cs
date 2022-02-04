using AutoMapper;
using GISA.ChangeDataCapture.Providers.Application.Contracts;
using GISA.ChangeDataCapture.Providers.Application.ViewModels;
using GISA.ChangeDataCapture.Providers.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.ChangeDataCapture.Providers.Application.Services
{
    public class Providerservice : ServiceBase, IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;

        public Providerservice(IMapper mapper, INotificationContext notificationContext, IProviderRepository providerRepository) : base(mapper, notificationContext)
        {
            _mapper = mapper;
            _providerRepository = providerRepository;
        }

        public async Task<IEnumerable<ProvidersimplifiedResponseViewModel>> GetAllAsync()
        {
            var providerList = await _providerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProvidersimplifiedResponseViewModel>>(providerList);
        }

        public async Task<ProviderResponseViewModel> GetByIdAsync(Guid id)
        {
            var provider = await _providerRepository.GetByIdAsync(id);
            return _mapper.Map<ProviderResponseViewModel>(provider);
        }
    }
}
