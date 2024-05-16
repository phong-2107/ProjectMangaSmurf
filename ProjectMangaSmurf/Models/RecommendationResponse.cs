namespace ProjectMangaSmurf.Models
{
    public class RecommendationResponse
    {
        public int Status { get; set; }
        public int[] recommended_books_id { get; set; }
    }
}
