namespace fidelappback.Requetes;

public record BaseResponse
{
    public string? Message { get; set; }
    public bool Success { get; set; }
}