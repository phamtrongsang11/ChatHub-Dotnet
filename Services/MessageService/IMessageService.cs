using System.Linq.Expressions;
using TeamChat.Models;

namespace TeamChat.Services.MessageService
{
    public interface IMessageService
    {
        Task<Message> Create(Message messEntity);
        Task<List<Message>> GetAll(string? includeProperties = null);
        Task<List<Message>> GetMessagesByChannelId(int cursor, string id);
        Task<Message?> Get(
            Expression<Func<Message, bool>> filter,
            string? includeProperties = null
        );

        Task<Message> Update(Message messEntity);

        Task<Message> PartialUpdate(string id, Message messEntity);

        Task<Boolean> isExist(string id);

        Task<Boolean> Remove(Message messEntity);
    }
}
