namespace fidelappback.Requetes.Client.Response;

public record ConfirmVisitClientResponse : BaseResponse
{
    public int? Score { get; set; }
    public int? Target { get; set; }
    public int? NbVisits { get; set; }
    public string? Descriptionpromotion { get; set; }
}