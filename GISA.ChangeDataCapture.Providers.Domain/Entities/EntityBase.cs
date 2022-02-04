using System;
using FluentValidation.Results;
using ServiceStack.DataAnnotations;

namespace GISA.ChangeDataCapture.Providers.Domain.Entities
{
    public abstract class EntityBase
    {
        [Alias("id")]
        [HashKey]
        public Guid Id { get; set; }
        [Alias("operation")]
        public string Operation { get; set; }
        [Alias("createdOn")]
        public DateTime? CreatedOn { get; set; }
        [Alias("updatedOn")]
        public DateTime? UpdatedOn { get; set; }
        [Alias("removedOn")]
        public DateTime? RemovedOn { get; set; }
        [Ignore]
        public bool Valid { get; private set; }
        [Ignore]
        public ValidationResult ValidationResult { get; private set; }

        public abstract ValidationResult GetValidationResult<TModel>();

        public bool Validate<TModel>(TModel model)
        {
            ValidationResult = GetValidationResult<TModel>();
            return Valid = ValidationResult.IsValid;
        }


    }
}
