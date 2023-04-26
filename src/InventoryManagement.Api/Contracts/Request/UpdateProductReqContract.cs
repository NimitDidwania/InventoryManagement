using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Api.Contracts.Request
{
    public class UpdateProductReqContract
    {
        public string ProductId{get;set;} = string.Empty;
        public int Quantity{get;set;}
        public int Price{get;set;}
    }

}