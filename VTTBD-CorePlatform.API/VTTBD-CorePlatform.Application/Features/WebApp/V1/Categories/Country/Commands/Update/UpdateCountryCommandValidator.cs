using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Update;

public sealed class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
    public UpdateCountryCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.Id)
                .NotEmpty()
                .WithMessage("Id must not be empty.");

            RuleFor(x => x.RequestData!.Name)
                .NotEmpty()
                .WithMessage("Name must not be empty.")
                .MaximumLength(200)
                .WithMessage("Name must not exceed 200 characters.");
        });
    }
}