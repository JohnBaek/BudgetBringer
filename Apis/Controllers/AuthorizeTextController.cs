using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apis.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class AuthorizeTextController
{
    [HttpGet]
    public string IsAuthorize()
    {
        return "Authorize";
    }
}