using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.ValidationsHelper
{
    public class ValidationHelper : ValidationAttribute
    {
        private readonly string _propertyName;

        public ValidationHelper(string propertyName)
        {
            _propertyName = propertyName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance!;
            var propertyInfo = instance.GetType().GetProperty(_propertyName);
            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(instance)!;
                if (propertyValue != null && string.IsNullOrWhiteSpace((string)value))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }

    }
}
