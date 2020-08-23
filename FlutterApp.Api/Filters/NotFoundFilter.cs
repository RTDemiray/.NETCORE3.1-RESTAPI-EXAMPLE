using FlutterApp.Api.DTOs;
using FlutterApp.Core.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.Filters
{
    public class NotFoundFilter<TEntity> : IAsyncActionFilter where TEntity : class
    {
        // Response 404 dönen hata kodlarını yakalamak için oluşturulmuş bir Error Handler
        private readonly IRepository<TEntity> _repository;

        public NotFoundFilter(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = context.ActionArguments.Values.FirstOrDefault();
            var entry = await _repository.GetByIdAsync(id);
            if (entry != null)
            {
                await next();
            }
            else
            {
                ErrorDto errorDto = new ErrorDto();
                errorDto.Status = 404;
                errorDto.Errors.Add($"id'si {id} olan veri bulunamadı!");
                context.Result = new NotFoundObjectResult(errorDto);
            }
        }
    }
}
