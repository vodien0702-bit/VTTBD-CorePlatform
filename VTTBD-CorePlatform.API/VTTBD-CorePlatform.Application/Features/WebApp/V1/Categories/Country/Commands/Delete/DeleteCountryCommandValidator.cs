using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Delete;

public sealed class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
{
    public DeleteCountryCommandValidator()
    {
        RuleFor(x => x.RequestData)
            .NotNull()
            .WithMessage("RequestData must not be null.");

        When(x => x.RequestData != null, () =>
        {
            RuleFor(x => x.RequestData!.Ids)
                .NotNull()
                .WithMessage("Ids must not be null.")
                .NotEmpty()
                .WithMessage("Ids must not be empty.");

            RuleForEach(x => x.RequestData!.Ids)
                .NotEmpty()
                .WithMessage("Id must not be empty.");
        });
    }
}