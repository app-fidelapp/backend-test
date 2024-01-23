using fidelappback.Requetes.Client.Request;
using fidelappback.Requetes.Client.Response;

namespace fidelappback.Services;
public interface IClientService
{
    Task<NewVisitClientResponse> RegisterClientAsync(NewVisitClientRequest request);
}