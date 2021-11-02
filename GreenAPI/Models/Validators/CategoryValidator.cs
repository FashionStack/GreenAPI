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
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(50);
        }
    }
}