using fidelappback.Database;
using fidelappback.Helpers;
using fidelappback.Models;
using fidelappback.Requetes;
using fidelappback.Requetes.User.Request;
using fidelappback.Requetes.User.Response;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.Services;

public class UserService : BaseService, IUserService
{
    public UserService(FidelappDbContext context) : base(context)
    {
    }

    // get user form database
    public async Task<User?> GetUserAsync(string? email, string? password)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

    // check if user can connect and update last connection
    public async Task<string?> LoginAsync(string? email, string? password)
    {
        var user = await GetUserAsync(email, password);
        if (user == null)
        {
            return null;
        }
        return await UpdateLastConnexionAsync(user);
    }

    public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
    {
        var response = new RegisterResponse 
        { 
            IsPhoneNumberAlreadyUsed = await PhoneNumberAlreadyExists(request.PhoneNumber), 
            IsEmailAlreadyUsed = await EmailAlreadyExistsAsync(request.Email),
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

    public async Task<UpdatePasswordResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
    {
        var response = new UpdatePasswordResponse(){
            IsPasswordToWeak = IsPasswordToWeak(request.NewPassword),
            IsLoginCorrect = await LoginAsync(request.Email, request.OldPassword) != null
        };

        if (!response.IsPasswordToWeak && response.IsLoginCorrect)
        {
            var userToUpdate = await GetUserAsync(request.Email, request.OldPassword) ?? new User();
            userToUpdate.Password = request.NewPassword;
            userToUpdate.ConnectionString = null;
            _context.SaveChanges();
        }

        return response;
    }

    private async Task<bool> EmailAlreadyExistsAsync(string? email)
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

    private async Task<string> UpdateLastConnexionAsync(User user)
    {
        // update last connection
        user.LastConnection = DateTime.Now;
        // if user has no ConnectString yet, create a new one
        if (string.IsNullOrEmpty(user.ConnectionString))
        {
            user.ConnectionString = EncryptHelper.ComputeSha1Hash(user);
            await _context.SaveChangesAsync();
        }
        return user.ConnectionString;
    }

    public async Task<UpdateUserResponse> UpdateUserAsync(User user, UpdateUserRequest request)
    {
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.Email = request.UserEmail;
        user.ShopName = request.ShopName;
        await _context.SaveChangesAsync();
        return new UpdateUserResponse();
    }
}