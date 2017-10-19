using GeradorDRG.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Filter
{
    public class ConfiguracaoAsyncActionFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var _context = (ApplicationDbContext)context.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerName;

            if (_context.Configuracao.FirstOrDefault() == null && controllerName != "Configuracoes" && (actionName != "Inicial"))
            {
                context.Result = new RedirectToActionResult("Inicial","Configuracoes", null);
            }
			else if (_context.Configuracao.FirstOrDefault() != null && controllerName=="Configuracoes" && (actionName=="Inicial"))
			{
				context.Result = new RedirectToActionResult("Edit", "Configuracoes", null);
			}
			else
            {
                await next();
            }

            //To do : before the action executes  
            //To do : after the action executes  
        }
    }
}
