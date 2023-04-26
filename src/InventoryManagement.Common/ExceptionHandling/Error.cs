using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common.ExceptionHandling
{
    public class Error
    {
        public int ErrorCode{get;set;} 
        public string Description{get;set;} = string.Empty ;
    }
}