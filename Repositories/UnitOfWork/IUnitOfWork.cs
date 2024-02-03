using TeamChat.Models;
using TeamChat.Repositories.BaseRepository;

namespace TeamChat.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Server> serverRepository { get; }
        IRepository<Profile> profileRepository { get; }
        IRepository<Member> memberRepository { get; }
        IRepository<Channel> channelRepository { get; }
        IRepository<Message> messageRepository { get; }
        IRepository<Conversation> conversationRepository { get; }
        IRepository<DirectMessage> directMessageRepository { get; }

        Task<int> Save();
    }
}
