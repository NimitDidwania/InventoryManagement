using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Auth.Models;
using InventoryManagement.Common.Filters;

namespace InventoryManagement.Auth.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Register(User user, string password, bool isAdmin);
        Task<TokenModel> Login(string username, string password);
        bool ValidateToken(string authToken);
        Task<List<User>> GetAllUsers(UserFilter userFilter);
        Task Delete(string id);
        Task<User> Update(User user, string id);
        Task<User> GetByUserId(string userId);
    }
}