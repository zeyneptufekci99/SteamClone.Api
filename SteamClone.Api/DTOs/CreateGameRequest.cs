namespace SteamClone.Api.DTOs
{
    public class CreateGameRequest
    {

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CoverImageUrl { get; set; } = null!;
    }
}
