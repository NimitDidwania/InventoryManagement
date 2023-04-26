using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Api.Contracts.Request;
using InventoryManagement.Api.Models;

namespace InventoryManagement.Api.Services.Interfaces
{
    public interface IInventoryServices
    {
         Task<Product> FetchProductById(string id);
         Task<Product> AddNewProduct(Product product);
         Task<Product> Update(string id, int qty, int price);
        
         Task<List<Product>> AllProducts(ProductFilterContract productFilterContract);
         Task Delete(string id);
         Task<Order> Buy(Order order);
         Task<List<Order>> GetAllOrders(string customerId);
         Task<Order> GetOrderById(string customerId, string orderId);
    }
}