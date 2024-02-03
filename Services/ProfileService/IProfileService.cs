using System.Linq.Expressions;
using TeamChat.Models;

namespace TeamChat.Services.ProfileService
{
    public interface IProfileService
    {
        Task<Profile> Create(Profile profileEntity);
        Task<List<Profile>> GetAll(string? includeProperties = null);

        Task<Profile?> Get(
            Expression<Func<Profile, bool>> filter,
            string? includeProperties = null
        );

        Task<Profile> Update(Profile profileEntiry);

        Task<Profile> PartialUpdate(string id, Profile profileEntiry);

        Task<Boolean> isExist(string id);

        Task<Boolean> Remove(Profile profileEntity);
    }
}
