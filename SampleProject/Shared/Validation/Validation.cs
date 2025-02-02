using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Shared.Validation
{
    public static class ValidationRequestModel
    {
        public static void Validate<T>(T model) where T : class
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, context, results))
            {
                throw new ValidationException(results.Select(r => r.ErrorMessage).ToList());
            }
        }
    }
}
