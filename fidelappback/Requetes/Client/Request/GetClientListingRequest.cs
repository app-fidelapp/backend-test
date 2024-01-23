namespace fidelappback.Requetes.Client.Request;

public record GetClientListingRequest : BaseRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    // search fields
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // sort fields 1 is ascendent, 0 or null is no filter, -1 is descendent
    public int? SortByFirstName { get; set; }
    public int? SortByLastName { get; set; }
    public int? SortByPhoneNumber { get; set; }
    public int? BirthDate { get; set; }
    public int? SortByLastVisit { get; set; }
    public int? SortByTotalScore { get; set; }
    public int? SortByTotalVisits { get; set; }
}