using System.ComponentModel;
using System.Net;
using AutoMapper;
using InventoryManagement.Api.Contracts;
using InventoryManagement.Api.Contracts.Request;
using InventoryManagement.Api.Contracts.Response;
using InventoryManagement.Api.Models;
using InventoryManagement.Api.Services.Interfaces;
using InventoryManagement.Common.ApiRoutes;
using InventoryManagement.Common.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[ApiController]
// [Route(Routes.Admin)]
// [Tags("Admin")]

public class InventoryController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IInventoryServices _inventoryServices;

    public InventoryController(IMapper mapper, IInventoryServices inventoryServices)
    {
        _mapper = mapper ;
        _inventoryServices = inventoryServices ;
    }

        ///<summary> List all the product </summary>
        /// <returns>The list of users.</returns>
    
    [HttpGet(Routes.GetAllProducts)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductResContract>))]
    public async Task<ActionResult<List<ProductResContract>>> AllProducts([FromQuery]ProductFilterContract productFilterContract){

        List<Product> x =  (await _inventoryServices.AllProducts(productFilterContract));
        List<ProductResContract> ans = new List<ProductResContract>();
        foreach(var y in x)ans.Add(_mapper.Map<ProductResContract>(y));
        return Ok(ans);
    }
    
        ///<summary>
        ///Get Product by its ProductId
        ///</summary>
        ///<param name="productId"> Write the Product Id of the product you want to get.</param>
    [HttpGet(Routes.GetProductById)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResContract))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public async Task<ActionResult<Product>> FetchProductById(string productId){

        var x =  await _inventoryServices.FetchProductById(productId);
        return Ok(x);
    }
    ///<summary> Add a new product </summary>
    [HttpPost(Routes.Product)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResContract))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<ActionResult<ProductResContract>> AddNewProduct(ProductReqContract product){
        if(Request.Headers["Role"] == "Customer")throw new UnauthorizedAccessException();

        return _mapper.Map<ProductResContract>(await _inventoryServices.AddNewProduct(_mapper.Map<Product>(product)));
    }
    ///<summary> Update a product </summary>
    [HttpPut(Routes.Product)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public async Task<ActionResult<Product>> Update([FromBody]UpdateProductReqContract product){
        if(Request.Headers["Role"] == "Customer")throw new UnauthorizedAccessException();

        return await _inventoryServices.Update(product.ProductId, product.Quantity, product.Price);
    }
    ///<summary> Delete a product by its ProductId</summary>
    
    ///<param name="productId"> Write the Product Id of the product you want to delete.</param>
    [HttpDelete(Routes.GetProductById)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public  async Task Delete(string productId)
    {
         if(Request.Headers["Role"] == "Customer")throw new UnauthorizedAccessException();
         await _inventoryServices.Delete(productId);
    }
    ///<summary>
    ///Get all the orders.
    ///</summary>
    [HttpGet("orders")]
        public async Task<ActionResult<List<OrderResContract>>> GetAllOrders(){
            string customerId = null;
            if(Request.Headers["Role"] == "Customer")customerId = Request.Headers["UserId"]!;
            var listModel = await _inventoryServices.GetAllOrders(customerId);
            List<OrderResContract> list = new List<OrderResContract>();
            foreach(var x in listModel){
                list.Add(_mapper.Map<OrderResContract>(x));
            }
            return Ok(list);
        }
        ///<summary>
        ///Get an order of a particular orderId
        ///</summary>
        /// <param name= "orderId"> Enter the orderId of the order you want to get.</param>
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<OrderResContract>> GetOrderById(string orderId){

            string customerId = null;
            if(Request.Headers["Role"] == "Customer")customerId = Request.Headers["UserId"]!;
            var order = _mapper.Map<OrderResContract>(await _inventoryServices.GetOrderById(customerId, orderId));
            
            return Ok(order);
        }
        ///<summary>
        ///To buy a product
        ///</summary>
        [HttpPut(Routes.BuyProduct)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResContract))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        public async Task<ActionResult<OrderResContract>> Buy(OrderReqContract order){
            string role = Request.Headers["Role"]!;
            if(role == "Admin")throw new UnauthorizedAccessException();
            string customerId = Request.Headers["UserId"]!;
            var orderModel = _mapper.Map<Order>(order);
            orderModel.CustomerId = customerId;
            var x =  _mapper.Map<OrderResContract>(await _inventoryServices.Buy(orderModel));
            return Ok(x);
        }
}
