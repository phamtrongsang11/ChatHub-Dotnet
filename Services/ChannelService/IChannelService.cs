using System.Linq.Expressions;
using TeamChat.Models;

namespace TeamChat.Services.ChannelService
{
    public interface IChannelService
    {
        Task<Channel> Create(Channel channelEntity);
        Task<List<Channel>> GetAll(string? includeProperties = null);

        Task<Channel?> Get(
            Expression<Func<Channel, bool>> filter,
            string? includeProperties = null
        );

        Task<Channel> Update(Channel channelEntiry);

        Task<Channel> PartialUpdate(string id, Channel channelEntiry);

        Task<Boolean> isExist(string id);

        Task<Boolean> Remove(Channel channelEntity);
    }
}
