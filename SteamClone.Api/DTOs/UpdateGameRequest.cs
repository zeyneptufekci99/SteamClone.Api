namespace SteamClone.Api.DTOs;

public class UpdateGameRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string CoverImage { get; set; } = null!;
}
