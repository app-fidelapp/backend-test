namespace fidelappback.Models;

public record CampagneSMS
{
    public string Message { get; set; }
    public List<Profil> PhoneNumbers { get; set; }
}