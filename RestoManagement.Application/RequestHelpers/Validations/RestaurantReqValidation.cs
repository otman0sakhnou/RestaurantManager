using FluentValidation;
using RestoManagement.Domain.DTOs;

namespace RestoManagement.Application.RequestHelpers.Validations;

public class RestaurantReqValidation : AbstractValidator<RestaurantReq>
{
    public RestaurantReqValidation()
    {
        RuleFor(x => x.Nom).NotEmpty().WithMessage("Le nom du restaurant est requis.");
        RuleFor(x => x.Adresse).NotEmpty().WithMessage("L'adresse du restaurant est requise.");
        RuleFor(x => x.Cuisine).NotEmpty().WithMessage("La cuisine du restaurant est requise.");
        RuleFor(x => x.Note).InclusiveBetween(0, 5).WithMessage("La note doit être entre 0 et 5.");
        RuleFor(x => x.Image).Must(x => x == null || (x.Length > 0 && x.ContentType.Contains("image"))).WithMessage("L'image doit être au format image.");
    }
}