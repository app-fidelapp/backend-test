namespace fidelappback.Requetes.User.Request;

public record LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}