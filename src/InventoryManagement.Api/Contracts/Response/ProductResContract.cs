

namespace InventoryManagement.Api.Contracts
{
    public class ProductResContract
    {
        public string ProductId{get;set;} = string.Empty;
        public string Name{get;set;} = string.Empty;
        public char Size{get;set;} 
        public int Price{get;set;} 
        public int Quantity{get;set;} 
    }
}