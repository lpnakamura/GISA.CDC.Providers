using GISA.ChangeDataCapture.Providers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.ChangeDataCapture.Providers.Domain.Contracts
{
    public interface IProviderRepository
    {
        Task<IEnumerable<Provider>> GetAllAsync();
        Task<Provider> GetByIdAsync(Guid id);
    }
}
