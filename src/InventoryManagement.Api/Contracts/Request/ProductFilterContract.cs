using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Api.Contracts.Request
{
    public class ProductFilterContract
    {
        ///<summary>Products will be filtered by their Name. Example- Shirt, Pant, etc.</summary>
        public string? Name{get;set;}
        ///<summary>Products will be filtered by their Size. Example- M, L, XL, etc.</summary>
        public string? Size{get;set;}
        ///<summary>Products will be filtered by their Minimum Price. Example- 200, 1000, etc.</summary>
        public int? MinPrice{get;set;}
        ///<summary>Products will be filtered by their Maximum Price. Example- 200, 1000, etc.</summary>
        public int? MaxPrice {get;set;}
    }
}