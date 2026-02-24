using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StringifyMaintenance.Models;

[Table("felhasznalo")]
public class User
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    [Column("Nev")]
    public string Nev { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    [Column("Jelszo")]
    public string Jelszo { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    [Column("SALT")]
    public string Salt { get; set; } = string.Empty;

    [Column("Jogosultsag")]
    public int Jogosultsag { get; set; } = 1;

    [Column("Aktiv")]
    public int Aktiv { get; set; } = 1;
}
