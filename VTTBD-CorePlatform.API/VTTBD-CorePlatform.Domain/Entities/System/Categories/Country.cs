using System;
using System.Collections.Generic;
using System.Text;
using VTTBD_CorePlatform.Domain.Common.Entities;

namespace VTTBD_CorePlatform.Domain.Entities.System.Categories;

public class Country : AggregateRoot<Guid>
{
    public string Name { get; private set; } = default!;

    // Constructor rỗng dành cho EF Core
    private Country() { }

    public Country(string name)
    {
        Id = Guid.NewGuid();
        Name = name.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name)
    {
        Name = name.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}