using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Template.Web.Controllers.Services
{
    public class ModelStateValidationsAttribute : ActionFilterAttribute
    {
        private readonly Type _viewModelType;

        public ModelStateValidationsAttribute(Type viewModelType)
        {
            _viewModelType = viewModelType;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey("model"))
            {
                context.Result = new BadRequestObjectResult("Invalid request");
                return;
            }

            var model = context.ActionArguments["model"];

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Any())
                    .SelectMany(x => x.Value.Errors.Select(e => new { Field = x.Key, Message = e.ErrorMessage }))
                    .ToList();

                var errorMessage = new StringBuilder();
                foreach (var error in errors)
                {
                    var displayName = GetDisplayNameForMember(error.Field, _viewModelType);
                    errorMessage.AppendLine($"Field: {displayName ?? error.Field} <br>");
                    errorMessage.AppendLine($"Error: {error.Message} <br>");
                }

                context.Result = new BadRequestObjectResult(errorMessage.ToString());
                return;
            }

            await next();
        }

        private string GetDisplayNameForMember(string memberName, Type viewModelType)
        {
            var property = viewModelType.GetProperty(memberName);
            if (property != null)
            {
                var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
                return displayNameAttribute?.DisplayName;
            }
            return null;
        }
    }
}
