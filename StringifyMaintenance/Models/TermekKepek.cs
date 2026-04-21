using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringifyMaintenance.Models;

[Table("termek_kepek")]
public class TermekKepek
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("TermekId")]
    public int TermekId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("kep1")]
    public string Kep1 { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("kep2")]
    public string Kep2 { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("kep3")]
    public string Kep3 { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("kep4")]
    public string Kep4 { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("kep5")]
    public string Kep5 { get; set; } = string.Empty;
}
