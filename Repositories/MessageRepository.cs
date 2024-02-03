using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories
{
    public class MessageRepository : Repository<Message>
    {
        private readonly ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
