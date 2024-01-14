namespace fidelappback.Requetes.User.Response;
public record UpdatePasswordResponse : BaseResponse
{
    public bool IsLoginCorrect { get; set; } = false;
    public bool IsPasswordToWeak { get; set; } = false;
}