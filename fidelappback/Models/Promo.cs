using System.ComponentModel.DataAnnotations;

namespace fidelappback.Models;

public record Promo
{
    [Key]
    [MaxLength(255)]
    public string? Id { get; set; }
    public User? User { get; set; }

    public PromoType Type { get; set; }

    [Range(0, int.MaxValue)]
    public int ScoreToGetPromo { get; set; }

    [MaxLength(500)]
    public string? PromoDescription { get; set; }
    public DateTime? CreatedAt { get; set; }
}