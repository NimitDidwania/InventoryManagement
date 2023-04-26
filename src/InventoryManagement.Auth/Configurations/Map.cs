using AutoMapper;
using InventoryManagement.Auth.Contracts.Request;
using InventoryManagement.Auth.Contracts.Response;
using InventoryManagement.Auth.Models;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.Filters;

namespace InventoryManagement.Auth.Configurations
{
    public class Map:Profile
    {
        public Map()
        {
            CreateMap<UserRequestContract, User>().ReverseMap();
            CreateMap<User,UserResponseContract>();
            CreateMap<User, UserDbo>().ReverseMap();
            CreateMap<TokenModel, TokenContract>();
            CreateMap<UserResponseContract, User>();
            CreateMap<UpdateUserRequestContract, User>();
            CreateMap<UserFilterContract, UserFilter>();
        }
    }
}