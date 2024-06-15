namespace ProjectMangaSmurf.Models.ViewModels
{
    public class FilterViewModel
    {
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string FilterType { get; set; }
        public int? Quarter { get; set; }
    }


}
