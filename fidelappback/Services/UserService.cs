using fidelappback.Database;
using fidelappback.Models;
using fidelappback.Requetes.User.Request;
using fidelappback.Requetes.User.Response;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.Services;

public class UserService : IUserService
{
    private readonly FidelappDbContext _context;

    public UserService(FidelappDbContext context)
    {
        _context = context;
    }

    // get user form database
    public async Task<User?> GetUser(string? email, string? password)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

    // check if user can connect and update last connection
    public async Task<Guid?> Login(string? email, string? password)
    {
        var user = await GetUser(email, password);
        if (user == null)
        {
            return null;
        }
        return UpdateLastConnexion(user);
    }

    public async Task<RegisterResponse> RegisterUser(RegisterRequest request)
    {
        var response = new RegisterResponse 
        { 
            IsPhoneNumberAlreadyUsed = await PhoneNumberAlreadyExists(request.PhoneNumber), 
            IsEmailAlreadyUsed = await EmailAlreadyExists(request.Email),
            IsPasswordToWeak = IsPasswordToWeak(request.Password)
        };

        
        // if they are all at false, create user
        if (!response.IsPhoneNumberAlreadyUsed && !response.IsEmailAlreadyUsed && !response.IsPasswordToWeak)
        {
            User newUser = (User)request;
            _context.User.Add(newUser);
            await _context.SaveChangesAsync();
        }

        return response;
    }

    public async Task<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request)
    {
        var response = new UpdatePasswordResponse(){
            IsPasswordToWeak = IsPasswordToWeak(request.NewPassword),
            IsLoginCorrect = await Login(request.Email, request.OldPassword) != null
        };

        if (!response.IsPasswordToWeak && response.IsLoginCorrect)
        {
            var userToUpdate = await GetUser(request.Email, request.OldPassword) ?? new User();
            userToUpdate.Password = request.NewPassword;
            userToUpdate.Guid = null;
            _context.SaveChanges();
        }

        return response;
    }

    private async Task<bool> EmailAlreadyExists(string? email)
    {
        // Check if email already exists
        var existingUserWithEmail = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        return existingUserWithEmail != null;
    }

    private async Task<bool> PhoneNumberAlreadyExists(string? phoneNumber)
    {
        // Check if phone number already exists
        var existingUserWithPhone = await _context.User.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        return existingUserWithPhone != null;
    }

    private bool IsPasswordToWeak(string? password)
    {
        // check if password is long enough, has at least one number, one uppercase, one lowercase
        return (password ?? "").Length < 8 
            || !(password ?? "").Any(char.IsDigit) 
            || !(password ?? "").Any(char.IsUpper) 
            || !(password ?? "").Any(char.IsLower);
    }

    private Guid UpdateLastConnexion(User user)
    {
        // update last connection
        user.LastConnection = DateTime.Now;
        // generate a new guid
        user.Guid = Guid.NewGuid();
        _context.SaveChanges();
        return (Guid)user.Guid;
    }
}