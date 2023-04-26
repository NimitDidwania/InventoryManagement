using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;

namespace InventoryManagement.Common.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<string> fetchHashedPassword(string username);
        Task  register(UserDbo user);
        Task<List<UserDbo>> GetAllUsers(UserFilter userFilter);
        Task Delete(string id);
        Task Update (UserDbo user);
        Task<int> FetchAdmin(string username);
        Task<UserDbo>GetByUserId(string userId);
        Task<bool> UsernameAlreadyExists(string username);
        Task<UserDbo> GetUserByUsername(string username);
    }
}