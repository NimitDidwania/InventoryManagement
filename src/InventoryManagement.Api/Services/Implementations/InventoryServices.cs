using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagement.Api.Contracts.Request;
using InventoryManagement.Api.Models;
using InventoryManagement.Api.Services.Interfaces;
using InventoryManagement.Common.CustomExceptions;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;
using InventoryManagement.Common.Logger;
using InventoryManagement.Common.Repositories.Interfaces;

namespace InventoryManagement.Api.Services.Implementations
{
    public class InventoryServices : IInventoryServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepo;
        private readonly ICustomLogger _logger;

        public InventoryServices(IMapper mapper, IInventoryRepository inventoryRepo, ICustomLogger logger, IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _inventoryRepo = inventoryRepo ;
            _logger = logger;
        }
        
        public async Task<Product> AddNewProduct(Product product)
        {
            try{
                bool sameProductExists = await _inventoryRepo.SameProductExists(product.Name, product.Size);
                if(sameProductExists == true)throw new BadRequestException() ;
                product.ProductId = Guid.NewGuid().ToString ();
                var x = _mapper.Map<Product>(await _inventoryRepo.AddNewProduct(_mapper.Map<ProductDbo>(product)));
                return x;
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            } 
        }

        public async Task<Product> FetchProductById(string id)
        {
            try{
                var  ans = _mapper.Map<Product>(await _inventoryRepo.FetchProductById(id));
                if(ans is null)throw new EntityNotFoundException();
                return ans;
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            }
        }

        public  async Task<Product> Update(string id, int qty, int price)
        {
            try{
                var product = await  _inventoryRepo.FetchProductById(id);
                if(product is null)throw new EntityNotFoundException();
                var ans = _mapper.Map<Product>(await _inventoryRepo.Update(id,qty, price));
                return ans;
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            }
            
        }
        

        public async Task<List<Product>> AllProducts(ProductFilterContract productFilterContract)
        {
            var x = await _inventoryRepo.AllProducts( _mapper.Map<ProductFilter>(productFilterContract));
            List<Product>list = new List<Product>();
            if(x is null)return list;
            foreach(var el in x){
                list.Add(_mapper.Map<Product>(el));
            }
            return list;
        }
        public async Task Delete(string id){
            try{
                var product = await  _inventoryRepo.FetchProductById(id);
                if(product is null)throw new EntityNotFoundException();
                await _inventoryRepo.Delete(id);
            }catch(Exception e){
                await _logger.Error(e);
                throw e;
            }

        }
        public async Task<Order> Buy(Order order){
            try{
                var product = await  _inventoryRepo.FetchProductById(order.ProductId);
                if(product is null)throw new EntityNotFoundException();
                int stockQuantity = product.Quantity;
                if(stockQuantity == 0)throw new BadRequestException();
                order.OrderId = Guid.NewGuid().ToString ();
                order.Date = DateTime.Now;
                await _inventoryRepo.Buy(_mapper.Map<OrderDbo>(order));
                return order;
            }
            catch(Exception e){
                await _logger.Error(e);
                throw e;
            }
        }
        public async Task<List<Order>> GetAllOrders(string customerId){
            var list = await _inventoryRepo.GetAllOrders(customerId);
            List<Order> ans = new List<Order>();
            foreach(var x in list){
                ans.Add(_mapper.Map<Order>(x));
            }
            return ans;
        }
        public async Task<Order> GetOrderById(string customerId, string orderId){
            
            var orderDbo = (await _inventoryRepo.GetOrderById(customerId, orderId));
            if(orderDbo is null){
                EntityNotFoundException e = new EntityNotFoundException();
                await _logger.Error(e);
                throw e;
            }
            if(customerId!=null && orderDbo.CustomerId != customerId){
                UnauthorizedAccessException e = new UnauthorizedAccessException();
                await _logger.Error(e);
                throw e;
            }
            return _mapper.Map<Order>(orderDbo);
        }
        
    }
}