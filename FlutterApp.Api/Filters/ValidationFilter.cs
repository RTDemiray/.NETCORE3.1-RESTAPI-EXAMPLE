using FlutterApp.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace FlutterApp.Api.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        // Modelde ki zorunlu alanları vb. kısıtları belirtmek için geriye model hatalarını dönen filter.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                ErrorDto errorDto = new ErrorDto();

                errorDto.Status = 400;

                IEnumerable<ModelError> modelErrors = context.ModelState.Values.SelectMany(v => v.Errors);

                modelErrors.ToList().ForEach(x =>
                {
                    errorDto.Errors.Add(x.ErrorMessage);
                });

                context.Result = new BadRequestObjectResult(errorDto);
            }
        }
    }
}
