using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using UGB.MVC.DTO.UsersDTO;

namespace UGB.MVC.Validations.UsersValidation
{
    public class CreateUserDTOValidation : AbstractValidator<CreateUserDTO>
    {
        public CreateUserDTOValidation()
        {
            RuleFor(x=>x.firstName).NotNull().WithMessage("El nombre está nulo");
            RuleFor(x=>x.firstName).NotEmpty().WithMessage("El nombre está vacío");
            
        }
    }
}