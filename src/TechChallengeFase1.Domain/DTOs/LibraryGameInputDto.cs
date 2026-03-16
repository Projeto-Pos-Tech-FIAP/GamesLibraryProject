using System;

namespace TechChallengeFase1.Domain.DTOs;

public class LibraryGameInputDto
{
    public int LibraryId { get; set; }
    public int GameId { get; set; }
    public int? AcquiredFromOrderId { get; set; }
    public DateTime? AcquiredAt { get; set; }
}
