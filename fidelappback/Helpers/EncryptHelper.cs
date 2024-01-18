using System.Security.Cryptography;
using System.Text;
using fidelappback.Models;

namespace fidelappback.Helpers;

public class EncryptHelper
{
    public string ComputeSha1Hash(User user)
    {
        var rawData = user.Email + user.Password + user.LastConnection;

        // Create a SHA1   
        using SHA1 sha1Hash = SHA1.Create();
        // ComputeHash - returns byte array  
        byte[] bytes = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        // Convert byte array to a string   
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        
        return builder.ToString();
    }
}