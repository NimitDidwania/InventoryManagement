using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Auth.Contracts.Request
{
    public class UserFilterContract
    {
        ///<summary> Filters the users by their Firstname, example- Nimit, Rajat, etc.</summary>
        public string? Firstname{get;set;}
        ///<summary> Filters the users by their Lastname, example- Didwania, Goel, etc.</summary>
        public  string? Lastname {get;set;}
        ///<summary> Filters the users by their Address, example- Lucknow, Delhi, etc.</summary>
        public string? Address{get;set;}
        ///<summary> Filters the users by their Age, example- 20, 21, etc.</summary>
        public string? Age {get;set;}
    }
}