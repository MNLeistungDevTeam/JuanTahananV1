using DMS.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using DMS.Application.Interfaces.Setup.ModuleRepository;

namespace DMS.Web.Controllers.Services
{
    public class ModuleServicesAttribute : ActionFilterAttribute
    {
        private readonly ModuleCodes _propertyName;
        private readonly Type _repositoryType;

        public ModuleServicesAttribute(ModuleCodes propertyName, Type repositoryType)
        {
            _propertyName = propertyName;
            _repositoryType = repositoryType;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var moduleRepo = context.HttpContext.RequestServices.GetService(_repositoryType);
            if (moduleRepo == null)
            {
                throw new ApplicationException($"{_repositoryType.Name} service not found.");
            }

            var modules = await ((IModuleRepository)moduleRepo).SpGetAllUserModules();
            string controllerName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();
            var module = modules.FirstOrDefault(x => x.Code == _propertyName.ToString() && x.Controller == controllerName && x.Action == actionName);
            if (module is not null)
            {
                var controller = context.Controller as Controller;
                controller.ViewData["Title"] = module.IsBreaded ? module.BreadName : "";
                var stringStr = "";
                foreach (var item in modules)
                {
                    if (module.IsBreaded)
                    {
                        //stringStr += @$"ñ<li class=""breadcrumb-item""><a asp-controller=""{module.Controller}"" asp-action=""{module.Action}"">@localizer[""{module.Description}""]</a></li>";
                    }
                }
                controller.ViewData["Breads"] = stringStr;
                switch (module.StatusName)
                {
                    case "InActive":
                    case "Maintenance":
                        context.Result = new RedirectToActionResult("BadRequestPage", "Home", null);
                        return;
                    default:
                        await next();
                        break;
                }
            }
            else
            {
                context.Result = new RedirectToActionResult("NotFoundPage", "Home", null);
            }
        }

    }
}
