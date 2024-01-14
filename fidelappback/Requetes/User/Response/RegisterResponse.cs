namespace fidelappback.Requetes.User.Response
{
    public record RegisterResponse : BaseResponse
    {
        public bool IsEmailAlreadyUsed { get; set; } = false;
        public bool IsPhoneNumberAlreadyUsed { get; set; } = false;
        public bool IsPasswordToWeak { get; set; } = false;
    }
}