using System;
using System.Collections.Generic;
using System.Text;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;

namespace VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;

/// <summary>
/// Aggregate Root – transaction boundary
/// </summary>
public abstract class AggregateRoot<TId> : SoftDeleteEntity<TId>
    where TId : notnull
{
    protected AggregateRoot() : base() { }

    protected AggregateRoot(TId id) : base(id) { }

}