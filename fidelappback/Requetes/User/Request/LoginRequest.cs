namespace fidelappback.Requetes.User.Request;

// define a record with email, password
public record LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}