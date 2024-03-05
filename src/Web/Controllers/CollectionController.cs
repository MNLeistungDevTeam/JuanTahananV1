using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

namespace DMS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : Controller
    {
        [HttpGet("GetControllers")]
        public IActionResult GetControllers()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var controllerTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract && type.Name.EndsWith("Controller"));
            return Ok(controllerTypes.Select(x => new { text = x.Name.Replace("Controller", "").ToUpper(), value = x.Name.Replace("Controller", "") }));
        }

        [HttpGet("GetActionsForController/{controllerName}")]
        public IActionResult GetActionsForController(string controllerName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllerType = assembly.GetTypes()
                .FirstOrDefault(type => type.Name.Equals($"{controllerName}Controller", StringComparison.OrdinalIgnoreCase));
            if (controllerType == null)
            {
                throw new ArgumentException("Controller not found");
            }
            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
               .Where(method => !method.IsSpecialName)
               .Select(method => method.Name)
               .ToList();
            return Ok(methods.Select(x => new { text = x.ToUpper(), value = x }));
        }
        [HttpGet("GetAllActions")]
        public IActionResult GetAllActions()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var controllerTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract && type.Name.EndsWith("Controller"));

            var allActions = new List<object>();

            foreach (var controllerType in controllerTypes)
            {
                var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(method => !method.IsSpecialName)
                    .Select(method => new { text = method.Name.ToUpper(), value = method.Name })
                    .ToList();

                allActions.AddRange(methods);
            }

            return Ok(allActions);
        }
    }
}
