using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Auth.Contracts.Response
{
    public class TokenContract
    {
        public string Token{get;set;} = string.Empty ;
        public DateTime Expire{get;set;}
    }
}