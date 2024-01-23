namespace fidelappback.Requetes.User.Request
{
    public record RegisterRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ShopName { get; set; }
    }
}