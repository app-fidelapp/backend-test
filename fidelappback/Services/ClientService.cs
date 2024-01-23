using fidelappback.Database;
using fidelappback.Models;
using fidelappback.Requetes.Client.Request;
using fidelappback.Requetes.Client.Response;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.Services;
public class ClientService : BaseService, IClientService
{

    private readonly IPromoService _promoService;

    public ClientService(FidelappDbContext context, IPromoService promoService) : base(context)
    {
        _promoService = promoService;
    }

    public async Task<ConfirmVisitClientResponse> ConfirmVisitClientAsync(ConfirmVisitClientRequest request, User user)
    {
        // get client from data base
        var client = _context.Client
            .Include(c => c.User)
            .FirstOrDefault(c => c.PhoneNumber == request.PhoneNumber && c.CountryNumber == request.CountryNumber && c.User!.Id == user.Id);
        
        if (client == null)
        {
            return new ConfirmVisitClientResponse
            {
                Success = false,
                Message = "Client not found"
            };
        }
        
        // get current fidelity card of the client 
        var card = _context.FidelityCard.Include(f => f.Promo).FirstOrDefault(f => f.Client!.Id == client!.Id && f.IsCompleted == false);
        if (card == null)
        {
            card = new FidelityCard
            {
                Client = client,
                Promo = await _promoService.GetPromoFromClientAsync(user),
                CreatedAt = DateTime.Now
            };

            await _context.FidelityCard.AddAsync(card);
        }

        // add score to the current card
        var rest = card.AddScore(request.Montant);

        // if card is completed, create a new one
        if (card.IsCompleted)
        {
            var newCard = new FidelityCard
            {
                Client = client,
                Promo = await _promoService.GetPromoFromClientAsync(user),
                CreatedAt = DateTime.Now
            };
            await _context.FidelityCard.AddAsync(newCard);

            // add rest to the new card
            newCard.AddScore(rest);
        }

        // update client last visit
        client.LastVisit = DateTime.Now;
        // update client total points
        client.TotalPoints += (int?)request.Montant ?? 0;
        // update client total visits
        client.TotalVisits += 1;
        // save changes
        await _context.SaveChangesAsync();

        return new ConfirmVisitClientResponse
        {
            Success = true,
            Score = card.Points,
            Target = card.Promo?.ScoreToGetPromo ?? 0,
            NbVisits = card.Visits,
            Descriptionpromotion = card.Promo?.PromoDescription ?? ""
        };
    }

    public async Task<NewVisitClientResponse> RegisterClientAsync(NewVisitClientRequest request, User user)
    {
        if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.CountryNumber))
        {
            return new NewVisitClientResponse
            {
                Success = false,
                Message = "Phone number null or empty"
            };
        }

        var client = await _context.Client
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.PhoneNumber == request.PhoneNumber && c.CountryNumber == request.CountryNumber && c.User!.Id == user.Id);   
        
        if(client == null)
        {
            var newClient = await CreateNewClientAsync(user, request.CountryNumber, request.PhoneNumber);
            if (newClient == null)
            {
                return new NewVisitClientResponse
                {
                    Success = false,
                    Message = "Promo need to be defined ! "
                };
            }
            client = newClient;
        }

        return new NewVisitClientResponse
        {
            Success = true,
            FirstName = client.FirstName,
            LastName = client.LastName,
            PhoneNumber = client.PhoneNumber,
            CountryNumber = client.CountryNumber
        };
    }

    // return Client if successfuly created a client with a fidelity card
    private async Task<Client?> CreateNewClientAsync(User user, string countryNumber, string phoneNumber)
    {
        // create a new client
        var client = new Client
        {
            PhoneNumber = phoneNumber,
            CountryNumber = countryNumber,
            User = user
        };

        // create a new fidelty card
        var card = new FidelityCard
        {
            Client = client,
            Promo = await _promoService.GetPromoFromClientAsync(user),
            CreatedAt = DateTime.Now
        };

        // if promo is not null, add client and fidelity card to database
        if (card.Promo != null)
        {
            _context.Client.Add(client);
            _context.FidelityCard.Add(card);
            await _context.SaveChangesAsync();
            return client;
        }
        return null;
    }

    public async Task<GetClientListingResponse?> GetClientListingAsync(GetClientListingRequest request, User user)
    {
        
        var query = _context.Client
            .Include(c => c.User)
            .Where(c => c.User!.Id == user.Id);

        // search filters

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            query = query.Where(c => (c.CountryNumber + c.PhoneNumber).Contains(request.PhoneNumber));
        }

        if (!string.IsNullOrEmpty(request.FirstName))
        {
            query = query.Where(c => c.FirstName!.Contains(request.FirstName));
        }

        if (!string.IsNullOrEmpty(request.LastName))
        {
            query = query.Where(c => c.LastName!.Contains(request.LastName));
        }

        // sort filters

        if (request.SortByFirstName != null && request.SortByFirstName != 0)
        {
            query = request.SortByFirstName == 1 ? query.OrderBy(c => c.FirstName) : query.OrderByDescending(c => c.FirstName);
        }

        if (request.SortByLastName != null && request.SortByLastName != 0)
        {
            query = request.SortByLastName == 1 ? query.OrderBy(c => c.LastName) : query.OrderByDescending(c => c.LastName);
        }

        if (request.SortByPhoneNumber != null && request.SortByPhoneNumber != 0)
        {
            query = request.SortByPhoneNumber == 1 ? query.OrderBy(c => c.PhoneNumber) : query.OrderByDescending(c => c.PhoneNumber);
        }

        if (request.BirthDate != null && request.BirthDate != 0)
        {
            query = request.BirthDate == 1 ? query.OrderBy(c => c.BirthDate) : query.OrderByDescending(c => c.BirthDate);
        }

        if (request.SortByLastVisit != null && request.SortByLastVisit != 0)
        {
            query = request.SortByLastVisit == 1 ? query.OrderBy(c => c.LastVisit) : query.OrderByDescending(c => c.LastVisit);
        }

        if (request.SortByTotalScore != null && request.SortByTotalScore != 0)
        {
            query = request.SortByTotalScore == 1 ? query.OrderBy(c => c.TotalPoints) : query.OrderByDescending(c => c.TotalPoints);
        }

        if (request.SortByTotalVisits != null && request.SortByTotalVisits != 0)
        {
            query = request.SortByTotalVisits == 1 ? query.OrderBy(c => c.TotalVisits) : query.OrderByDescending(c => c.TotalVisits);
        }

        // pagination
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);
        var skip = (request.Page - 1) * request.PageSize;
        query = query.Skip(skip).Take(request.PageSize);

        // get clients
        var clients = await query.ToListAsync();

        var clientResponses = new List<ClientResponse>();

        foreach (var c in clients)
        {
            var card = _context.FidelityCard.Include(f => f.Promo).FirstOrDefault(f => f.Client!.Id == c!.Id && f.IsCompleted == false);
            clientResponses.Add(new ClientResponse
            {
                PhoneNumber = c.PhoneNumber,
                FirstName = c.FirstName,
                LastName = c.LastName,
                BirthDate = c.BirthDate,
                LastVisit = c.LastVisit,
                CurrentScore = card != null ? card.GetScore() : 0,
                TotalScore = c.TotalPoints,
                TotalVisits = c.TotalVisits
            });
        }

        return new GetClientListingResponse
        {
            Success = true,
            TotalPages = totalPages,
            TotalItems = totalItems,
            Clients = clientResponses
        };
    }
}