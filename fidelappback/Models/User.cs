using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fidelappback.Requetes.User.Request;

namespace fidelappback.Models;
public record User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [MaxLength(15)]
    public string? PhoneNumber { get; set; }

    [MaxLength(50)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(100)]
    public string? Password { get; set; }

    [MaxLength(50)]
    public string? ShopName { get; set; }
    
    public DateTime? LastConnection { get; set; }

    // mobile guid
    public String? ConnectionString { get; set; }

    public bool IsActivated { get; set; } = true;


    // implicit conversion from RegisterRequest to user
    public static explicit operator User(RegisterRequest request)
    {
        return new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Password = request.Password,
            ShopName = request.ShopName,
            IsActivated = true
        };
    }
}