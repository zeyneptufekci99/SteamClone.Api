using SteamClone.Api.Models;
using SteamClone.Api.Repositories;

namespace SteamClone.Api.Data;

public class ReviewService
{
    private readonly ReviewRepository _reviewRepository;

    public ReviewService(ReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<bool> HasUserReviewedGameAsync(string userId, string gameId)
    {
        return await _reviewRepository.HasUserReviewedGameAsync(userId, gameId);
    }

    public async Task CreateAsync(Review review)
    {
        await _reviewRepository.CreateAsync(review);
    }

    public async Task<List<Review>> GetByGameIdAsync(string gameId)
    {
        return await _reviewRepository.GetByGameIdAsync(gameId);
    }

    public async Task<double> GetAverageRatingAsync(string gameId)
    {
        var reviews = await _reviewRepository.GetByGameIdAsync(gameId);

        if (!reviews.Any())
            return 0;

        return reviews.Average(x => x.Rating);
    }

    public async Task<int> GetReviewCountAsync(string gameId)
    {
        return (int)await _reviewRepository.GetReviewCountAsync(gameId);
    }
}