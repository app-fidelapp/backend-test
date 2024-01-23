namespace fidelappback.Requetes.Client.Response;

public record GetClientListingResponse : BaseResponse
{
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public IEnumerable<ClientResponse>? Clients { get; set; }
}

public record ClientResponse
{
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? LastVisit { get; set; }
    public int? CurrentScore { get; set; }
    public int? TotalScore { get; set; }
    public int? TotalVisits { get; set; }
}