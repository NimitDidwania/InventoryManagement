using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using InventoryManagement.Common.CustomExceptions;
using InventoryManagement.Common.Logger;

namespace InventoryManagement.Api.Middlewares
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
            string token = _context.Request.Headers.Authorization!;
            
            if(token == null || token.Count()<=7){
                UnauthorizedAccessException e = new UnauthorizedAccessException();
                await _logger.Error(e);
                throw e;
            }
            string path =  _context.Request.Path;

            var jwt = token;
            var handler = new JwtSecurityTokenHandler();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try{
                var myToken = handler.ReadJwtToken(jwt);
                var x = myToken.Claims.ToArray() ;
                string endpoint = "user/validate" ;
                string userId = x[1].Value;
                _context.Request.Headers["UserId"] = userId;
                _context.Request.Headers["Role"] = x[0].Value;
                var result = await _client.GetAsync(endpoint);
                if(result.IsSuccessStatusCode){
                    // if(x[0].Value == "Admin"){
                    //     if(path[1] != 'a'){
                    //         throw new CaughtException();
                    //     }
                    // }
                    // if(x[0].Value == "Consumer"){
                    //     if(path[1] == 'a'){
                    //         throw new CaughtException();
                    //     }
                    // }
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