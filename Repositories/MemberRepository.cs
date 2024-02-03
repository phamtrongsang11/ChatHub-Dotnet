using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories
{
    public class MemberRepository : Repository<Member>
    {
        private readonly ApplicationDbContext _db;

        public MemberRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
