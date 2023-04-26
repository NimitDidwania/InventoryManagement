using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common.Filters
{
    public class ProductFilter
    {
        public string? Name{get;set;}
        public string? Size{get;set;}
        public int? MinPrice{get;set;}
        public int? MaxPrice {get;set;}
    }
}