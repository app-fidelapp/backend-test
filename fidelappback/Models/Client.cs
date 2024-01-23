using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fidelappback.Models;

public record Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User? User { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(20)]
    public string? CountryNumber { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    [Range(0, int.MaxValue)]
    public int TotalVisits { get; set; } = 0;

    [Range(0, int.MaxValue)]
    public int TotalPoints { get; set; } = 0;
    public bool AcceptSMS { get; set; } = true;
}