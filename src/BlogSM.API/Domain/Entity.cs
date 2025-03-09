using System;

namespace BlogSM.API.Domain;

public class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();
}
