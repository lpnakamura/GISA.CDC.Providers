using GISA.ChangeDataCapture.Providers.Application.Contracts;
using GISA.ChangeDataCapture.Providers.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.ChangeDataCapture.Providers.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProviderController : ApiBaseController
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService, INotificationContext notificationContext) : base(notificationContext)
        {
            _providerService = providerService;
        }

        [HttpGet("{id}")]
        public async Task<ProviderResponseViewModel> GetByIdAsync(Guid id)
        {
            return await _providerService.GetByIdAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<ProvidersimplifiedResponseViewModel>> GetAllAsync()
        {
            return await _providerService.GetAllAsync();
        }
    }
}
