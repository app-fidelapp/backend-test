namespace fidelappback.Requetes;

public record BaseRequest
{
    // fields to identify the user
    public string? UserEmail { get; set; }
    public string? UserConnectionString { get; set; }
}