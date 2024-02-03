using System.Linq.Expressions;
using TeamChat.Models;

namespace TeamChat.Services.ConversationService
{
    public interface IConversationService
    {
        Task<Conversation> Create(Conversation converEntity);
        Task<List<Conversation>> GetAll(string? includeProperties = null);

        Task<Conversation?> Get(
            Expression<Func<Conversation, bool>> filter,
            string? includeProperties = null
        );

        Task<Conversation> Update(Conversation converEntity);

        Task<Conversation> PartialUpdate(string id, Conversation converEntity);

        Task<Boolean> isExist(string id);

        Task<Boolean> Remove(Conversation converEntity);
    }
}
