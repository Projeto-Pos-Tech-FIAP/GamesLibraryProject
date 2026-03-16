using System;

namespace TechChallengeFase1.Domain.DTOs;

public class LibraryInputDto
{
    public Guid UserGuid { get; set; }
    public Guid CreatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
