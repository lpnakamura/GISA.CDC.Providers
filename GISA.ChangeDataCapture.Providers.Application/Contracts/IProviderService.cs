using GISA.ChangeDataCapture.Providers.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.ChangeDataCapture.Providers.Application.Contracts
{
    public interface IProviderService
    {
        Task<IEnumerable<ProvidersimplifiedResponseViewModel>> GetAllAsync();
        Task<ProviderResponseViewModel> GetByIdAsync(Guid id);
    }
}
