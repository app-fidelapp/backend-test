namespace fidelappback.Requetes.Client.Response;

public record NewVisitClientResponse : BaseResponse
{
    public bool? IsNewClient { get; set; }
    // completed if the client is already in the database
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CountryNumber { get; set; }
}