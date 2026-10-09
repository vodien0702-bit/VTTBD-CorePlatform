using System;
using System.Linq.Expressions;
using CountryEntity = VTTBD_CorePlatform.Domain.Entities.System.Categories.Country;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;

public class CountryByIdForUpdateSpec
{
    public Expression<Func<CountryEntity, bool>> Criteria { get; }

    public CountryByIdForUpdateSpec(Guid id)
    {
        Criteria = x => x.Id == id;
    }
}