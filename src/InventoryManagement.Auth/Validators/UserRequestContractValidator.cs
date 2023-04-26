using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using InventoryManagement.Auth.Contracts.Request;

namespace InventoryManagement.Auth.Validators
{
    public class UserRequestContractValidator:AbstractValidator<UserRequestContract>
    {
        public UserRequestContractValidator()
        {
            RuleFor(u => u.Username).NotNull().NotEmpty() ;
            RuleFor(u => u.Password).NotNull().NotEmpty().Length(8,15) ;
            RuleFor(u => u.Mail).EmailAddress();
        }
    }
}