using Microsoft.AspNetCore.Mvc;

namespace SteamClone.Api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem(
            title: "Unexpected error",
            statusCode: 500
        );
    }
}