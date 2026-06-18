namespace SteamClone.Api.DTOs;

public class CreateReviewRequest
{
    public string GameId { get; set; } = null!;
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
}
