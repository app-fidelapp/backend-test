namespace fidelappback.Requetes.User.Response
{
    public record LoginResponse : BaseResponse
    {
        public string? ConnectionString { get; set; }
    }
}