using GISA.ChangeDataCapture.Providers.Domain.Contracts;
using GISA.ChangeDataCapture.Providers.Domain.Entities;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Aws.DynamoDb;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GISA.ChangeDataCapture.Providers.Infrastructure.Data.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly IPocoDynamo _pocoDynamo;
        private readonly IConfiguration _configuration;

        public ProviderRepository(IPocoDynamo pocoDynamo, IConfiguration configuration)
        {
            _pocoDynamo = pocoDynamo;
            _configuration = configuration;
            RegisterTable();
        }

        public async Task<IEnumerable<Provider>> GetAllAsync()
        {
            return await _pocoDynamo.GetAllAsync<Provider>();
        }

        public async Task<Provider> GetByIdAsync(Guid id)
        {
            return await _pocoDynamo.GetItemAsync<Provider>(id);
        }

        private void RegisterTable()
        {
            var providerType = typeof(Provider);
            providerType.AddAttributes(new AliasAttribute(_configuration.GetSection("Aws:Dynamo:TableName").Value));
            _pocoDynamo.RegisterTable(providerType);
            _pocoDynamo.InitSchema();
        }
    }
}
