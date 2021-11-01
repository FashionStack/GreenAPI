using FluentValidation;
using GreenAPI.Models.Validators.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenAPI.Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.CategoryId).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(100);
            RuleFor(p => p.Size).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(5);
            RuleFor(p => p.SKU).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(50);
            RuleFor(p => p.ReferenceCode).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(50);
            RuleFor(p => p.Amount).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
            RuleFor(p => p.Sustainable).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
            RuleFor(p => p.ImageUrl).NotNull().NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(255);
        }
    }
}