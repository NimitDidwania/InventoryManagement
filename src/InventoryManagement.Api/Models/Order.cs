using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Api.Models
{
    public class Order
    {
        public string OrderId{get;set;} = string.Empty ;
        public string CustomerId{get;set;} = string.Empty ;
        public string ProductId{get;set;} = string.Empty ;
        public DateTime Date{get;set;} 
    }
}