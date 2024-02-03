using System.Linq.Expressions;
using TeamChat.Models;

namespace TeamChat.Services.DirectMessageService
{
    public interface IDirectMessageService
    {
        Task<DirectMessage> Create(DirectMessage directEntity);
        Task<List<DirectMessage>> GetAll(string? includeProperties = null);

        Task<DirectMessage?> Get(
            Expression<Func<DirectMessage, bool>> filter,
            string? includeProperties = null
        );

        Task<List<DirectMessage>> GetDirectMessagesByConversationId(string id);

        Task<DirectMessage> Update(DirectMessage directEntity);

        Task<DirectMessage> PartialUpdate(string id, DirectMessage directEntity);

        Task<Boolean> isExist(string id);

        Task<Boolean> Remove(DirectMessage directEntity);
    }
}
