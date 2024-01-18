using System.ComponentModel.DataAnnotations;

namespace fidelappback.Models;

public record FidelityCard
{
    [Key]
    [MaxLength(255)]
    public string? Id { get; set; }
    public Client? Client { get; set; }
    public Promo? Promo { get; set; }

    [Range(0, int.MaxValue)]
    public int Points { get; set; }

    [Range(0, int.MaxValue)]
    public int Visits { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsCompleted { get; set; } = false;

    public bool IsColected { get; set; } = false;
}