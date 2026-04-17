using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("felhasznalo")]
public class User
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Nev")]
    public string Nev { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("Jelszo")]
    public string Jelszo { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    [Column("SALT")]
    public string Salt { get; set; } = string.Empty;

    [Column("Jogosultsag")]
    public int Jogosultsag { get; set; }

    [Column("Aktiv")]
    public bool Aktiv { get; set; } = true;
}