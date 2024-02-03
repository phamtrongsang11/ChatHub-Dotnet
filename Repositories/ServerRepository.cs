using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories
{
    public class ServerRepository : Repository<Server>
    {
        private readonly ApplicationDbContext _db;

        public ServerRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
