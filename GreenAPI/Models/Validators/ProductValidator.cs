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
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
            RuleFor(p => p.Name).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(100);
            RuleFor(p => p.Size).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(5);
            RuleFor(p => p.SKU).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(50);
            RuleFor(p => p.ReferenceCode).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(50);
            RuleFor(p => p.Amount).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
            RuleFor(p => p.Sustainable).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
            RuleFor(p => p.ImageUrl).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired)
                .MaximumLength(255);
        }
    }
}