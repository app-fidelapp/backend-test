namespace fidelappback.Requetes.CampagneSMSMultiple;

public class RequestCampagneSMSMultiple
{
    public string Message { get; set; }
    public List<string> PhoneNumbers { get; set; }
}