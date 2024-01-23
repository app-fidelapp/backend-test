using fidelappback.Models;

namespace fidelappback.Services;
public interface IPromoService
{
    Task<Promo?> GetPromoFromClientAsync(User user);
}