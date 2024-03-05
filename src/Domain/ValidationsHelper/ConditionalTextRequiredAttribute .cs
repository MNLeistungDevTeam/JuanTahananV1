using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.ValidationsHelper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConditionalTextRequiredAttribute : ValidationAttribute
    {
        private readonly string _checkBoxProperty;
        private readonly string _textInputProperty;

        public ConditionalTextRequiredAttribute(string checkBoxProperty, string textInputProperty, string errorMessage)
        {
            _checkBoxProperty = checkBoxProperty;
            _textInputProperty = textInputProperty;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var checkBoxProperty = instance.GetType().GetProperty(_checkBoxProperty);
            var textInputProperty = instance.GetType().GetProperty(_textInputProperty);

            if (checkBoxProperty != null && textInputProperty != null)
            {
                var checkBoxValue = (bool)checkBoxProperty.GetValue(instance)!;
                var textInputValue = (string)textInputProperty.GetValue(instance)!;

                if (checkBoxValue && string.IsNullOrWhiteSpace(textInputValue))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
