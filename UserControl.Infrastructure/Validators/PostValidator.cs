using FluentValidation;
using UserControl.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserControl.Infrastructure.Validators
{
    public class PostValidator : AbstractValidator<PostDto>
    {
        public PostValidator()
        {
            //RuleFor(post => post.Description)
            //    .NotNull()
            //    .Length(10, 500);

            RuleFor(post => post.Description)
               .NotNull()
               .WithMessage("La descripción no puede ser nula")
               .Length(10, 500)
               .WithMessage("la longitud debe estar entre 10 y 500 caracteres");

            RuleFor(post => post.Date)
                .NotNull()
                .LessThan(DateTime.Now);

        }
    }
}
