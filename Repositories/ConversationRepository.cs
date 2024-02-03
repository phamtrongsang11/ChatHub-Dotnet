using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories
{
    public class ConversationRepository : Repository<Conversation>
    {
        private readonly ApplicationDbContext _db;

        public ConversationRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
