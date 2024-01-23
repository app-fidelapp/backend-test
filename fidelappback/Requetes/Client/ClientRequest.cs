namespace fidelappback.Requetes.Client;

public record ClientRequest : BaseRequest
{
    public string? PhoneNumber { get; set; }
    public string? CountryNumber { get; set; }
    public double? Montant { get; set; }
}