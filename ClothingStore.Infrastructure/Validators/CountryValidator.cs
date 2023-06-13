using ClothingStore.Core.DTOs;
using FluentValidation;

namespace ClothingStore.Infrastructure.Validators
{
    public class CountryValidator : AbstractValidator<CountryDto>
    {
        public CountryValidator()
        {
            RuleFor(country => country.Name)
                .NotNull()
                .WithMessage("El nombre del pais no puede ser nulo");

            RuleFor(country => country.Name)
                .Length(1, 20)
                .WithMessage("La longitud del nombre del pais debe estar entre 1 y 20 caracteres");
        }
    }

}
