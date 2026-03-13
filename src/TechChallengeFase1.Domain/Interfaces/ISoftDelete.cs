using System;
using System.Collections.Generic;
using System.Text;

namespace TechChallengeFase1.Domain.Interfaces;

public interface ISoftDelete
{
    bool IsDeleted { get; }
}
