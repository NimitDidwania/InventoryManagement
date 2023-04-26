using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Auth.Contracts.Response
{
    public class UserResponseContract
    {
        public string UserId{get;set;} = string.Empty;
        public string Firstname{get;set;} = string.Empty;
        public string Lastname{get;set;} = string.Empty;       
        public string Address{get;set;} = string.Empty;
        public string Username{get;set;} = string.Empty;
        public string Mail{get;set;} = string.Empty;
        public string Phone{get;set;} = string.Empty;
        public int Age{get;set;} 
    }
}