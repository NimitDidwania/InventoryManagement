using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using InventoryManagement.Api.Contracts;

namespace InventoryManagement.Api.Validators
{
    public class ProductRequestContractValidator:AbstractValidator<ProductReqContract>
    {
        public ProductRequestContractValidator()
        {
            RuleFor(u => u.Name).NotNull().NotEmpty();
            RuleFor(u => u.Price).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(u => u.Size).NotNull().NotEmpty();
            RuleFor(u => u.Quantity).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}