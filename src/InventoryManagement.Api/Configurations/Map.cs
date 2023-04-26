using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagement.Api.Contracts;
using InventoryManagement.Api.Contracts.Request;
using InventoryManagement.Api.Contracts.Response;
using InventoryManagement.Api.Models;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;

namespace InventoryManagement.Api.Configurations
{
    public class Map:Profile
    {
        public Map()
        {
            CreateMap<ProductReqContract, Product>();
            CreateMap<Product, ProductResContract>();
            CreateMap<Product, ProductDbo>().ReverseMap();
            CreateMap<OrderReqContract, Order>();
            CreateMap<Order, OrderResContract>();
            CreateMap<Order, OrderDbo>().ReverseMap();
            CreateMap<ProductFilterContract, ProductFilter>();
        }
    }
}