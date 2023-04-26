using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagement.Auth.Models;
using InventoryManagement.Auth.Services.Interfaces;
using InventoryManagement.Common.CustomExceptions;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;
using InventoryManagement.Common.Logger;
using InventoryManagement.Common.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagement.Auth.Services.Implementations
{
    public class UserService:IUserService
    {
        private string key;
        private readonly ICustomLogger _logger;
        
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, IUserRepository userRepo, IConfiguration configuration, ICustomLogger logger)
        {
            _mapper = mapper ;
            _userRepo = userRepo ;
            _configuration = configuration ;
            key = configuration.GetSection("ConnectionStrings:Key").Value!.ToString();
            _logger = logger;
            
        }
        private string HashMyPassword(string password){
            string keyFactorString = ( _configuration.GetSection("ConnectionStrings:KeyFactor").Value!.ToString());
            int keyFactor = Int32.Parse(keyFactorString) ;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, keyFactor) ;
            return passwordHash;
        }
        private TokenModel CreateToken(bool isAdmin, string userId){
            
            List<Claim>claims = new List<Claim>();

            if(isAdmin){
             Claim claim1 = new Claim(ClaimTypes.Role, "Admin");
             Claim claim2 = new Claim(JwtRegisteredClaimNames.Sub, userId);
             claims.Add(claim1);
             claims.Add(claim2);
            }
            else{ 
                Claim claim1 = new Claim(ClaimTypes.Role, "Consumer");
                Claim claim2 = new Claim(JwtRegisteredClaimNames.Sub, userId);
                claims.Add(claim1);
                claims.Add(claim2);
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var secToken = new JwtSecurityToken(
                claims : claims,
                signingCredentials: credentials,
                issuer: _configuration.GetSection("ConnectionStrings:Issuer").Value!.ToString(),
                expires: DateTime.UtcNow.AddDays(1));

            var handler = new JwtSecurityTokenHandler();
            TokenModel myToken = new TokenModel();
            myToken.Token =  handler.WriteToken(secToken);
            myToken.Expire = secToken.ValidTo;
            return myToken ;
        }

        public async Task<TokenModel> Login(string username, string password)
        {
            try
            {
                var user = _mapper.Map<User>(await _userRepo.GetUserByUsername(username));
                if(user is null)throw new UnauthorizedAccessException();
                
                    
                if(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)){
                        return  CreateToken(user.isAdmin == 1, user.UserId);
                }
                else throw new UnauthorizedAccessException();
            }
            catch(UnauthorizedAccessException e){
                await _logger.Error(e);
                throw e;
            }
        }

        public async Task<User> Register(User user, string password, bool isAdmin)
        {
            try{
                if(await _userRepo.UsernameAlreadyExists(user.Username) == true){
                    throw new BadRequestException();
                }
                
                user.PasswordHash = HashMyPassword(password);
                if(isAdmin)
                user.isAdmin = 1 ;
                else user.isAdmin = 0;

                user.UserId =  Guid.NewGuid().ToString ();
                await _userRepo.register(_mapper.Map<UserDbo>(user));
                return user;
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            }
            
        }

        public  bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        private  TokenValidationParameters GetValidationParameters()
        {
            
            return new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateLifetime = true, 
                ValidateIssuer = true,   
                ValidIssuer = _configuration.GetSection("ConnectionStrings:Issuer").Value!.ToString() ,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)) 
            };
        }
        public async Task<List<User>> GetAllUsers(UserFilter userFilter){
            List<UserDbo> allUserDbo =  await _userRepo.GetAllUsers( userFilter);
            List<User> allUser = new List<User>();
            foreach(var x in allUserDbo){
                allUser.Add(_mapper.Map<User>(x));
            }
            return allUser;
        }
        public async Task Delete(string id){
            try{
                var user = await _userRepo.GetByUserId(id);
                if(user is null)throw new EntityNotFoundException();
                await _userRepo.Delete(id);
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            }
        }
        public async Task<User> Update(User user, string id){
            try{
                var savedUser = await _userRepo.GetByUserId(id);
                if(savedUser is null)throw new EntityNotFoundException();
                if( savedUser.Username != user.Username && await _userRepo.UsernameAlreadyExists(user.Username) == true)throw new BadRequestException();
                user.UserId = id;
                Exception e = new Exception(user.Address);
                await _logger.Error(e);
                await _userRepo.Update(_mapper.Map<UserDbo>(user));
                return user;
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            }
        }
        public async Task<User> GetByUserId(string userId){
            try{
                var x = await _userRepo.GetByUserId(userId);
                var ans = _mapper.Map<User>(x);
                return ans;
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            }
        }
    }
}