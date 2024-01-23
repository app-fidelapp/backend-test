using fidelappback.Database;
using fidelappback.Models;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.Services;

public class PromoService : BaseService, IPromoService
{
    public PromoService(FidelappDbContext context) : base(context)
    {
    }

    public async Task<Promo?> GetPromoFromClientAsync(User user)
    {
        return await _context.Promo
            .Include(p => p.User)
            .OrderBy(p => p.CreatedAt)
            .FirstOrDefaultAsync(p => p.User!.Id == user.Id);
    }
}