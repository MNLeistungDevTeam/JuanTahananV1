using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.ValidationsHelper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CheckboxMultiValidationHelper : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public CheckboxMultiValidationHelper(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var properties = _propertyNames.Select(propertyName => instance.GetType().GetProperty(propertyName)).ToList();

            var trueCount = properties.Count(property => (bool)property.GetValue(instance)!);

            if (trueCount == 0)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
