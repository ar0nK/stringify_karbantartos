using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringifyMaintenance.Models;

[Table("rendeles")]
public class Rendeles
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("FelhasznaloId")]
    public int FelhasznaloId { get; set; }

    [Column("Osszeg")]
    public int Osszeg { get; set; }

    [Required]
    [MaxLength(32)]
    [Column("Status")]
    public string Status { get; set; } = string.Empty;

    [Column("Datum")]
    public DateTime? Datum { get; set; }
}
