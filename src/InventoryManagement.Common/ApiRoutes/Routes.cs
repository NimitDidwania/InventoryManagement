using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common.ApiRoutes
{
    public static class Routes
    {
        public const string GetAllProducts=  "products" ;
        public const string GetProductById = "product/{productId}";
        public const string BuyProduct =  "product/buy";
        public const string Product = "product";
        public const string Consumer = "consumer";
        public const string Admin = "admin";
        public const string GetAllUsers = "users";
        public const string GetByUserId = "user/{userId}";
        public const string LoginUser = "user/login";
        public const string RegisterUser = "user/register";
        public const string Validate = "user/validate";
    }
}