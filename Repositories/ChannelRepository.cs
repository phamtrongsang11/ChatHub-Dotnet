using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories
{
    public class ChannelRepository : Repository<Channel>
    {
        private readonly ApplicationDbContext _db;

        public ChannelRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
