using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories
{
    public class DirectMessageRepository : Repository<DirectMessage>
    {
        private readonly ApplicationDbContext _db;

        public DirectMessageRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
