using fidelappback.Models;
using fidelappback.Requetes.User.Request;
using fidelappback.Requetes.User.Response;

namespace fidelappback.Services;
public interface IUserService
{
    Task<User?> GetUser(string? email, string? password);
    Task<Guid?> Login(string? email, string? password);
    Task<RegisterResponse> RegisterUser(RegisterRequest request);
}