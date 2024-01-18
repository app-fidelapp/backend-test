using fidelappback.Database;

namespace fidelappback.Services;
public class ClientService : BaseService, IClientService
{
    public ClientService(FidelappDbContext context) : base(context)
    {
    }
}