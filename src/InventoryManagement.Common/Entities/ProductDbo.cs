using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common.Entities
{
    public class ProductDbo
    {
        public string ProductId{get;set;} = string.Empty;
        public string Name{get;set;} = string.Empty;
        public char Size{get;set;} 
        public int Price{get;set;} 
        public int Quantity{get;set;} 
    }
}