using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;
using InventoryManagement.Common.Repositories.Interfaces;

namespace InventoryManagement.Common.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectContext _context;

        public UserRepository(ProjectContext context)
        {
            _context = context ;
        }
        public async Task<string> fetchHashedPassword(string username){
            var list = _context.Users.Where(x => x.Username == username).ToList();
            if(list.Count()!=1)return "";
            return list[0].PasswordHash ;
        }

        
        public async Task  register(UserDbo user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserDbo>> GetAllUsers(UserFilter userFilter){
            var list =  _context.Users.ToList();
            if(userFilter.Firstname is not null){
               list = list.Where(x => x.Firstname == userFilter.Firstname ).ToList();
            }
            if(userFilter.Lastname is not null){
                list = list.Where(x => x.Lastname == userFilter.Lastname).ToList();
            }
            if(userFilter.Address is not null ){
                list = list.Where(x => x.Address == userFilter.Address).ToList();
            }
            if(userFilter.Age is not null){
                list = list.Where(x => x.Age == Int32.Parse(userFilter.Age)).ToList();
            }
            return list;
        }
        public async Task Delete(string id){
            var userDbo = await _context.Users.FindAsync(id);
            _context.Users.Remove(userDbo);
            await _context.SaveChangesAsync();
        }
        public async Task Update (UserDbo user){
            var x = await _context.Users.FindAsync(user.UserId);
            user.PasswordHash = x.PasswordHash ;
            _context.Users.Remove(x);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<int> FetchAdmin(string username){
            var list = _context.Users.Where(x => x.Username == username).ToList();
            if(list.Count()!=1)return -1;
            if(list[0].isAdmin == 1)return 1;
            return 0;
        }
        public async Task<UserDbo> GetByUserId(string userId){
            var user = await _context.Users.FindAsync(userId);
            return user;
        }
        public async Task<bool> UsernameAlreadyExists(string username){
            var list = _context.Users.Where(x=> x.Username == username) ;
            if(list is null || list.Count()==0)return false;
            return true;
        }
        public async Task<UserDbo> GetUserByUsername(string username){
            var list = _context.Users.Where(x=> x.Username == username) ;
            if(list is null || list.Count()!=1)return null;
            return list.ToList()[0];
        }
    }
}