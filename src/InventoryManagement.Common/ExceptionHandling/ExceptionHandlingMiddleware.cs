using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Common.CustomExceptions;
using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Common.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next ;
        }
        public async Task Invoke(HttpContext context){
            try{
                await _next(context);
            }
            catch(UnauthorizedAccessException e){
                Error error = new Error();
                error.Description = "Unauthorized" ;
                error.ErrorCode = 401 ;
                context.Response.StatusCode = error.ErrorCode;
                await context.Response.WriteAsJsonAsync(error);
            }
            catch(EntityNotFoundException e){
                Error error = new Error();
                error.Description = "Entity Not Found" ;
                error.ErrorCode = 404 ;
                context.Response.StatusCode = error.ErrorCode;
                await context.Response.WriteAsJsonAsync(error);
            }
            catch(BadRequestException e){
                Error error = new Error();
                error.Description = "Bad Request" ;
                error.ErrorCode = 400 ;
                context.Response.StatusCode = error.ErrorCode;
                await context.Response.WriteAsJsonAsync(error);
            }
        }
 
    }
}