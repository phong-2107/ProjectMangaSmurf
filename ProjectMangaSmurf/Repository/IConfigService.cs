namespace ProjectMangaSmurf.Repository
{
    public interface IConfigService
    {
        Task<string> GetConfigValueAsync(int id);
    }

}
