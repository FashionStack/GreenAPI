using FluentValidation;
using GreenAPI.Models.Validators.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
            RuleFor(p => p.SizeType).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
        }
    }
}