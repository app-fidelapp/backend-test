namespace fidelappback.Requetes.User.Request;
public record UpdateUserRequest : BaseRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ShopName { get; set; }
}