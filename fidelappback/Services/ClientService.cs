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
        var client = _context.Client.FirstOrDefault(c => c.PhoneNumber == request.PhoneNumber && c.CountryNumber == request.CountryNumber && c.User!.Id == user.Id);
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
                Promo = _promoService.GetPromoFromClientAsync(user).Result,
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
                Promo = _promoService.GetPromoFromClientAsync(user).Result,
                CreatedAt = DateTime.Now
            };
            await _context.FidelityCard.AddAsync(newCard);

            // add rest to the new card
            newCard.AddScore(rest);
        }

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

        var client = await _context.Client.Include(c => c.User).FirstOrDefaultAsync(c => c.PhoneNumber == request.PhoneNumber && c.CountryNumber == request.CountryNumber && c.User!.Id == user.Id);   
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
}