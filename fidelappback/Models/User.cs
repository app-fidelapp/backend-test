using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fidelappback.Models;
public class User
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

    public string? Guid { get; set; }

}