using ProjectMangaSmurf.Models;

namespace ProjectMangaSmurf.Repository
{
    public interface IWebMediaRepository
    {
        WebMediaConfig GetFirstAsync();
    }
}
