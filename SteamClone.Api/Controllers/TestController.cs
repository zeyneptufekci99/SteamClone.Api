using Microsoft.AspNetCore.Mvc;
using SteamClone.Api.Data;

namespace SteamClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class  TestController: ControllerBase
{
    private readonly MongoDbService _mongoDbService;
    public TestController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "MongoDB bağlantısı başarılı!",
            database = _mongoDbService.Database.DatabaseNamespace.DatabaseName
        });
    }
}


