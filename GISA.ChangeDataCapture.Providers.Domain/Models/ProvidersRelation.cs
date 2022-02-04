using System;

namespace GISA.ChangeDataCapture.Providers.Domain.Models
{
    public class ProvidersRelation
    {
        public Provider Provider { get; set; }
        public Speciality Speciality { get; set; }
        public City City { get; set; }
    }

    public class Provider
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

    public class Speciality
    {
        public string Id { get; set; }
        public string Acronym { get; set; }
        public string Title { get; set; }
    }

    public class City
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public Int64? Latitude { get; set; }
        public Int64? Longitude { get; set; }
    }
}
