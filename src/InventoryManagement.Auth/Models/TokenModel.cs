using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Auth.Models
{
    public class TokenModel
    {
        public string Token{get;set;} = string.Empty ;
        public DateTime Expire{get;set;}
    }
}