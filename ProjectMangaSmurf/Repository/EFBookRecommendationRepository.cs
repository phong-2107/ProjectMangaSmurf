
using ProjectMangaSmurf.Models;
using System.Net.Http;

namespace ProjectMangaSmurf.Repository
{
    public class EFBookRecommendationRepository : IBookRecommendationRepository
    {
        private readonly HttpClient _httpClient;

        public EFBookRecommendationRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000/"); // Change this to the Flask app's URL if hosted
        }


        public async Task<List<int>> GetRecommendationsAsync(int bookId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/goi-y-sach", new Dictionary<string, int> { { "Book-Id", bookId } });
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RecommendationResponse>();
                    return result.recommended_books_id.ToList() ?? new List<int>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error from API: {errorContent}");
                    return new List<int>();  // Optionally handle different status codes differently
                }
            }
            catch (HttpRequestException ex)
            {
                // Log or handle exceptions if the request failed
                Console.WriteLine($"An error occurred connecting to the API: {ex.Message}");
                return new List<int>();
            }
        }
    }
}
