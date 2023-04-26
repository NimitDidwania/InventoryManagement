using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Auth.Contracts.Request
{
    public class LoginRequestContract
    {
        public string Username{get;set;}=string.Empty;
        public string Password{get;set;}=string.Empty;
    }
}