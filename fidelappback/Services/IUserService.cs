using fidelappback.Models;
using fidelappback.Requetes;
using fidelappback.Requetes.User.Request;
using fidelappback.Requetes.User.Response;

namespace fidelappback.Services;
public interface IUserService
{
    Task<User?> GetUserAsync(string? email, string? password);
    Task <User?> IsAuthorizedAsync(BaseRequest request);
    Task<string?> LoginAsync(string? email, string? password);
    Task<RegisterResponse> RegisterUserAsync(RegisterRequest request);
    Task<UpdatePasswordResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
    Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
}