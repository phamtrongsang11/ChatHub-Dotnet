using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories
{
    public class ProfileRepository : Repository<Profile>
    {
        private readonly ApplicationDbContext _db;

        public ProfileRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
