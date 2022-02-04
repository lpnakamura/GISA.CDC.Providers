using System;

namespace GISA.ChangeDataCapture.Providers.Application.ViewModels
{
    public class ProviderResponseViewModel
    {
        public Guid Id { get; set; }
        public ProviderRelationViewModel Before { get; set; }
        public ProviderRelationViewModel After { get; set; }
        public string Operation { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? RemovedOn { get; set; }
    }

    public class ProviderRelationViewModel
    {
        public ProviderViewModel Provider { get; set; }
        public SpecialityViewModel Speciality { get; set; }
        public CityViewModel City { get; set; }
    }

    public class ProviderViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Nationality { get; set; }
        public string ProfessionalIdentifier { get; set; }
        public string Address { get; set; }
        public string AddressNumber { get; set; }
        public string Addition { get; set; }
    }

    public class SpecialityViewModel
    {
        public string Id { get; set; }
        public string Acronym { get; set; }
        public string Title { get; set; }
    }

    public class CityViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public Int64? Latitude { get; set; }
        public Int64? Longitude { get; set; }
    }
}
