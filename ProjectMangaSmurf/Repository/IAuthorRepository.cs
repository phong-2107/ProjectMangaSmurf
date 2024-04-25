using Microsoft.EntityFrameworkCore;
using ProjectMangaSmurf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMangaSmurf.Repository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<TacGium>> GetAllAuthors();
        Task<TacGium> GetAuthorById(string id); // Thay vì GetAuthorsByID()
        Task AddAsync(TacGium ath);
        Task UpdateAsync(TacGium ath);
        Task DeleteAsync(string id);
         Task<string> GenerateAuthorId();
        Task<List<BoTruyen>> GetComicsByAuthorId(string authorId);

        Task DeleteAllChaptersAndDetails(string idBo);
        Task<IEnumerable<TacGium>> GetAllAuthorsFilteredByActiveStatus(bool isActive);


 }
}
