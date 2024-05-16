namespace ProjectMangaSmurf.Repository
{
    public interface IBookRecommendationRepository
    {
        Task<List<int>> GetRecommendationsAsync(int bookId);
    }
}
