using System;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Domain.Entities;

public abstract class SoftDelete : ISoftDelete
{
    public bool IsDeleted { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Restore()
    {
        IsDeleted = false;
    }
}
