using FluentValidation.Results;
using ServiceStack.DataAnnotations;
using System;

namespace GISA.ChangeDataCapture.Providers.Domain.Entities
{
    public class Provider : EntityBase
    {
        [Alias("before")]
        public ProviderRelationEntity Before { get; set; }
        [Alias("after")]
        public ProviderRelationEntity After { get; set; }

        public override ValidationResult GetValidationResult<TModel>()
        {
            return new ValidationResult();
        }
    }

    public class ProviderRelationEntity
    {
        [Alias("provider")]
        public ProviderEntity Provider { get; set; }
        [Alias("speciality")]
        public SpecialityEntity Speciality { get; set; }
        [Alias("city")]
        public CityEntity City { get; set; }
    }

    public class ProviderEntity
    {
        [Alias("id")]
        public string Id { get; set; }
        [Alias("name")]
        public string Name { get; set; }
        [Alias("maritalStatus")]
        public string MaritalStatus { get; set; }
        [Alias("birthDay")]
        public DateTime? BirthDay { get; set; }
        [Alias("nationality")]
        public string Nationality { get; set; }
        [Alias("professionalIdentifier")]
        public string ProfessionalIdentifier { get; set; }
        [Alias("address")]
        public string Address { get; set; }
        [Alias("addressNumber")]
        public string AddressNumber { get; set; }
        [Alias("addition")]
        public string Addition { get; set; }
    }

    public class SpecialityEntity
    {
        [Alias("id")]
        public string Id { get; set; }
        [Alias("acronym")]
        public string Acronym { get; set; }
        [Alias("title")]
        public string Title { get; set; }
    }

    public class CityEntity
    {
        [Alias("id")]
        public string Id { get; set; }
        [Alias("name")]
        public string Name { get; set; }
        [Alias("state")]
        public string State { get; set; }
        [Alias("latitude")]
        public Int64? Latitude { get; set; }
        [Alias("longitude")]
        public Int64? Longitude { get; set; }
    }
}
