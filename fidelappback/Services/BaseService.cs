using fidelappback.Database;
using fidelappback.Models;
using fidelappback.Requetes;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.Services;
public class BaseService
{
    protected readonly FidelappDbContext _context;

    public BaseService(FidelappDbContext context)
    {
        _context = context;
    }

    public async Task<User?> IsAuthorizedAsync (BaseRequest request)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);
        // compare connection string from request and database
        if (user?.ConnectionString != request.ConnectionString)
        {
            return null;
        }
        return user;
    }
}