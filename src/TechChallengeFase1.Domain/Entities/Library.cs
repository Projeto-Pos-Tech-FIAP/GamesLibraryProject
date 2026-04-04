using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechChallengeFase1.Domain.Interfaces;

namespace TechChallengeFase1.Domain.Entities;

[Table("Library")]
public class Library : SoftDelete
{
    [Key]
    public int LibraryId { get; set; }

    public Guid UserGuid { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; private set; }

    public ICollection<LibraryGame> LibraryGames { get; set; } = new List<LibraryGame>();
}
