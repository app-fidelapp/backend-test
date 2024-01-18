namespace fidelappback.Requetes;

public record BaseRequest
{

    // fields to identify the user
    public string? Email { get; set; }
    public string? ConnectionString { get; set; }
}