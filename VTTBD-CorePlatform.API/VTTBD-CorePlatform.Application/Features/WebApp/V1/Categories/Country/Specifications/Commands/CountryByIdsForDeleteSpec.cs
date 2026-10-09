using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CountryEntity = VTTBD_CorePlatform.Domain.Entities.System.Categories.Country;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;

public class CountryByIdsForDeleteSpec
{
    public Expression<Func<CountryEntity, bool>> Criteria { get; }

    public CountryByIdsForDeleteSpec(List<Guid> ids)
    {
        Criteria = x => ids.Contains(x.Id);
    }
}