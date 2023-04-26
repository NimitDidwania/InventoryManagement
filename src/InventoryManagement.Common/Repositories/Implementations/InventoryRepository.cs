using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;
using InventoryManagement.Common.Repositories.Interfaces;

namespace InventoryManagement.Common.Repositories.Implementations
{
    public class InventoryRepository:IInventoryRepository
    {
        private readonly ProjectContext _context;
        private readonly string showAll;

        public InventoryRepository(ProjectContext context)
        {
            _context = context;
            showAll = "all";
        }
        public async Task<ProductDbo> FetchProductById(string id)
        {
            var product =   await _context.Products.FindAsync(id);
            return product;
        }
        public async Task<ProductDbo> AddNewProduct(ProductDbo product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductDbo>Update(string id, int qty, int price)
        {
            var product = await _context.Products.FindAsync(id);
            product!.Quantity =qty;
            product!.Price =price;
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<List<ProductDbo>> AllProducts(ProductFilter productFilter){
             var list =  _context.Products.ToList() ;
             if(!string.IsNullOrEmpty(productFilter.Name)){
                list = list.Where(x => x.Name == productFilter.Name).ToList();
             }
             if(!string.IsNullOrEmpty(productFilter.Size)){
                list = list.Where(x => x.Size == productFilter.Size[0]).ToList();
             }
             if(productFilter.MinPrice is not null){
                list = list.Where(x => x.Price >= productFilter.MinPrice).ToList();
             }
             if(productFilter.MaxPrice is not null){
                list = list.Where(x => x.Price <= productFilter.MaxPrice).ToList();
             }
             return list;
        }
        public async Task Delete(string id){
            var x = await _context.Products.FindAsync(id);
            _context.Products.Remove(x);
            await _context.SaveChangesAsync();
        }
        //check 
        public async Task Buy(OrderDbo order){
            var x = await _context.Products.FindAsync(order.ProductId);
            x.Quantity--;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> SameProductExists(string name, char size){
            List<ProductDbo> list = _context.Products.Where(x => x.Name == name && x.Size == size).ToList();
            if(list is null || list.Count() == 0)return false;
            return true;
        }
        public async Task<List<OrderDbo>> GetAllOrders(string customerId){
            if(string.IsNullOrEmpty(customerId))return _context.Orders.ToList();
            List<OrderDbo> list = _context.Orders.Where(x => x.CustomerId == customerId).ToList();
            return list;
        }
        public async Task<OrderDbo> GetOrderById(string customerId, string orderId){
            var order = await _context.Orders.FindAsync(orderId);
            return order;
         }
    }
}