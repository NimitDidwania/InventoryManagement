using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common.Entities
{
    public class OrderDbo
    {
        public string OrderId{get;set;} = string.Empty;
        public  string ProductId { get; set; } =string.Empty; 
        public string CustomerId{get;set;} = string.Empty ;
        public DateTime Date{get;set;} 
    }
}