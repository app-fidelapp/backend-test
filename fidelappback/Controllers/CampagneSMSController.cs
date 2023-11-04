using fidelappback.Enum;
using fidelappback.Helpers;
using fidelappback.Requetes.CampagneSMSMultiple;
using fidelappback.Requetes.CampagneSMSUnique;
using Microsoft.AspNetCore.Mvc;

namespace fidelappback.controllers;

[Route("api/[controller]")]
[ApiController]
public class CampagneSMSController : ControllerBase
{

    // call this endpoint to see if the OVH API is working and the name of the service (affiché dans la console)
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        OvhSMSHelper smsHelper = new OvhSMSHelper();
        var status = await smsHelper.GetSMSServices();
        return Ok();
    }

    // call this API to send message to one user
    // exemple de body :
    // {
    //    "Message": "Je sais où tu habites...",
    //    "Profil": {
    //        "PhoneNumber": "0485264651",
    //        "Name": "justin Smagghe"
    //     }
    // }
    [HttpPost("unique")]
    public async Task<IActionResult> CampagneSMSUnique([FromBody] RequestCampagneSMSUnique request)
    {
        OvhSMSHelper smsHelper = new OvhSMSHelper();
        var status = await smsHelper.SendSMS(request.Message, request.Profil);
        if(status == Status.ok)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    // TODO : work in progress
    [HttpPost("multiple")]
    public IActionResult CampagneSMSMultiple([FromBody] RequestCampagneSMSMultiple request)
    {
        return NotFound();
    }
}