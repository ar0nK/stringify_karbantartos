using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringifyMaintenance.Models;

[Table("termek")]
public class Termek
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("Nev")]
    public string Nev { get; set; } = string.Empty;

    [Required]
    [Column("Leiras")]
    public string Leiras { get; set; } = string.Empty;

    [MaxLength(255)]
    [Column("RovidLeiras")]
    public string? RovidLeiras { get; set; }

    [Column("Ar")]
    public int Ar { get; set; }

    [Column("Elerheto")]
    public bool Elerheto { get; set; }

    [Column("GitarTipusId")]
    public int? GitarTipusId { get; set; }

    [Column("Letrehozva")]
    public DateTime? Letrehozva { get; set; }
}
