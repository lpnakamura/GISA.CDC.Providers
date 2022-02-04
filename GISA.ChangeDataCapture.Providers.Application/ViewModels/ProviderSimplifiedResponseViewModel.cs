using System;

namespace GISA.ChangeDataCapture.Providers.Application.ViewModels
{
    public class ProvidersimplifiedResponseViewModel
    {
        public Guid Id { get; set; }
        public ProviderSimplifiedRelationViewModel Before { get; set; }
        public ProviderSimplifiedRelationViewModel After { get; set; }
        public string Operation { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? RemovedOn { get; set; }
    }

    public class ProviderSimplifiedRelationViewModel
    {
        public ProviderSimplifiedViewModel Provider { get; set; }
    }

    public class ProviderSimplifiedViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDay { get; set; }
        public string ProfessionalIdentifier { get; set; }
    }
}
