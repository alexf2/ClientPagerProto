using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CqsDataFoundation.Validation
{
    public static class ValidationHelper
    {
        public static void Validate<T>(T obj, ValidationContext ctx)
        {
            var errors = new List<ValidationResult>();
            bool res = Validator.TryValidateObject(obj, ctx, errors, true);
            if (!res)
            {
                throw new AggregateException(errors.Select((e) => new ValidationException(e is CompositeValidationResult ?
                    e.ErrorMessage + ": " + string.Join("; ", (e as CompositeValidationResult).Results.Select(v => v.ErrorMessage)) :
                    e.ErrorMessage))
                );
            }
        }
    }
}