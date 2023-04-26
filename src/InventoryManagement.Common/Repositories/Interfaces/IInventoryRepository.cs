using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;

namespace InventoryManagement.Common.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
         Task<ProductDbo> FetchProductById(string id);
         Task<ProductDbo> AddNewProduct(ProductDbo product);
         Task<ProductDbo> Update(string id, int qty, int price);
         Task<List<ProductDbo>> AllProducts(ProductFilter productFilter);
         Task Delete(string id);
         Task Buy(OrderDbo order);
        Task<bool> SameProductExists(string name, char size);
        Task<List<OrderDbo>> GetAllOrders(string customerId);
         Task<OrderDbo> GetOrderById(string customerId, string orderId);
    }
}