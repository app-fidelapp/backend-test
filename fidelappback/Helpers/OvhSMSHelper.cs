using System.Text;
using System.Security.Cryptography;
using fidelappback.Enum;
using fidelappback.Models;

namespace fidelappback.Helpers;

public class OvhSMSHelper
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string _AK = "a844460e174f7760";
    private readonly string _AS = "9d2461873ae580cb6b4bd2335e70035c";
    private readonly string _CK = "21c1745966fcaafea92a549000d4742d";
    private readonly string _baseurl = "https://eu.api.ovh.com/1.0/sms/";

    public async Task<Status> SendSMS(string? message, Profil? profil)
    {
        if(message == null || profil == null)
        {
            return Status.error;
        }
        // var serviceName = "sms-XX00000-1"; // sms-gi59168-1, j'obtiens toujours cette valeur en faisant un get sur le service
        var phoneNumber = profil.PhoneNumber;
        var method = "POST";
        var priority = "high";
        // var senderForResponse = true;
        var sender = "FidelApp";
        var fullMessage = "Salut " + profil.Name + ", " + message + " !";
        var body = "{ \"charset\": \"UTF-8\", \"receivers\": [ \"" + phoneNumber + "\" ], \"message\": \"" + fullMessage + "\", \"priority\": \"" + priority + "\",  \"senderForResponse\": true, \"sender\": \"" + sender + "\"}";
        var fullUrl = _baseurl + "sms-gi59168-1/jobs";
        return await OVH_SMS_API(method, body, fullUrl);
    }

    public static string HashSHA1(string sInputString)
    {
        SHA1 sha = SHA1.Create();
        byte[] bHash = sha.ComputeHash(Encoding.UTF8.GetBytes(sInputString));
        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < bHash.Length; i++)
        {
            sBuilder.Append(bHash[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }

    private async Task<Status> OVH_SMS_API(string method, string body, string urlAPI)
    {
        try 
        {
            var query = urlAPI;
            var unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var timestamp = unixTimestamp.ToString();

            String signature = "$1$" + HashSHA1(_AS + "+" + _CK + "+" + method + "+" + query + "+" + body + "+" + timestamp);

            // Set headers
            client.DefaultRequestHeaders.Add("X-Ovh-Application", _AK);
            client.DefaultRequestHeaders.Add("X-Ovh-Consumer", _CK);
            client.DefaultRequestHeaders.Add("X-Ovh-Signature", signature);
            client.DefaultRequestHeaders.Add("X-Ovh-Timestamp", timestamp);

            // Set content
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(urlAPI, content);
            var answer = await response.Content.ReadAsStringAsync();
  
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return Status.ok;
            }
            return Status.error;
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
            return Status.error;
        }
    }

    public async Task<Status> GetSMSServices()
    {
        var method = "GET";
        var body = "";
        var fullUrl = _baseurl;
        return await OVH_SMS_API(method, body, fullUrl);
    }
}