using TeamChat.Database;
using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext _db;
        public IRepository<Server> serverRepository { get; private set; }
        public IRepository<Profile> profileRepository { get; private set; }
        public IRepository<Member> memberRepository { get; private set; }
        public IRepository<Channel> channelRepository { get; private set; }
        public IRepository<Message> messageRepository { get; private set; }
        public IRepository<Conversation> conversationRepository { get; private set; }
        public IRepository<DirectMessage> directMessageRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            serverRepository = new ServerRepository(_db);
            profileRepository = new ProfileRepository(_db);
            memberRepository = new MemberRepository(_db);
            channelRepository = new ChannelRepository(_db);
            messageRepository = new MessageRepository(_db);
            conversationRepository = new ConversationRepository(_db);
            directMessageRepository = new DirectMessageRepository(_db);
        }

        public Task<int> Save()
        {
            return _db.SaveChangesAsync();
        }
    }
}
