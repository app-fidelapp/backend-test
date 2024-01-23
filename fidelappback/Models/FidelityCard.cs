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
    public int Points { get; set; } = 0;

    [Range(0, int.MaxValue)]
    public int Visits { get; set; } = 0;

    public DateTime? CreatedAt { get; set; }

    public bool IsCompleted { get; set; } = false;

    public bool IsCollected { get; set; } = false;

    internal int AddScore(double? montant)
    {
        switch (Promo?.Type)
        {
            case PromoType.Points:
                Points += (int?)montant ?? 0;
                CheckCompletion();
                return Points - (Promo?.ScoreToGetPromo ?? 0);
            case PromoType.Visits:
                Visits += 1;
                CheckCompletion();
                return 0;
            default:
                return 0;
        }
    }

    internal int GetScore()
    {
        switch (Promo?.Type)
        {
            case PromoType.Points:
                return Points;
            case PromoType.Visits:
                return Visits;
            default:
                return 0;
        }
    }

    private void CheckCompletion()
    {
        IsCompleted = (Promo?.Type) switch
        {
            PromoType.Points => Points >= (Promo?.ScoreToGetPromo ?? 0),
            PromoType.Visits => Visits >= (Promo?.ScoreToGetPromo ?? 0),
            _ => false
        };
    }
}