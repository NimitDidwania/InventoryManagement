using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InventoryManagement.Common.CustomExceptions;
using InventoryManagement.Common.Logger;

namespace InventoryManagement.Auth.Middlewares
{
    public class TokenValidation
    {
        private readonly RequestDelegate _next;
        
        private readonly HttpClient _client;
        private readonly ICustomLogger _logger;

        public TokenValidation( RequestDelegate next, ICustomLogger logger, IConfiguration configuration)
        {
            
            _next = next;
            _client = new HttpClient();
            string address = configuration.GetSection("AuthApiBaseAddress").Value!.ToString();
            _client.BaseAddress = new Uri(address);
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext _context){
            string path =  _context.Request.Path;
            if(path == "/user/register" || path == "/user/login" || path == "/user/validate"){
                await _next(_context);
            }
            else{
                string token = _context.Request.Headers.Authorization!;
                if(token == null){
                    UnauthorizedAccessException e = new UnauthorizedAccessException();
                    await _logger.Error(e);
                    throw e;
                }
                var jwt = token;
                var handler = new JwtSecurityTokenHandler();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try{
                    var myToken = handler.ReadJwtToken(jwt);
                    var x = myToken.Claims.ToArray() ;
                    string endpoint = "user/validate" ;
                    if(x[0].Value != "Admin")throw new UnauthorizedAccessException();
                    var result = await _client.GetAsync(endpoint);
                    if(result.IsSuccessStatusCode){
                        await _next(_context);
                    }
                    else{
                        throw new CaughtException();
                    }
                }
                catch(CaughtException e){
                    UnauthorizedAccessException ex = new UnauthorizedAccessException();
                    await _logger.Error(ex);
                    throw ex;
                }
            }
            
        }
    }
}