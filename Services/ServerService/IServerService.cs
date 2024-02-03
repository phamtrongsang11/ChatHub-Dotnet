using System.Linq.Expressions;
using TeamChat.Models;

namespace TeamChat.Services.ServerService
{
    public interface IServerService
    {
        Task<Server> Create(Server serverEntity);
        Task<List<Server>> GetAll(string? includeProperties = null);

        Task<Server?> Get(Expression<Func<Server, bool>> filter, string? includeProperties = null);

        Task<Server> Update(Server serverEntiry);

        Task<Server> PartialUpdate(string id, Server serverEntiry);

        Task<Boolean> isExist(string id);

        Task<Boolean> Remove(Server serverEntity);

        Task<Server?> FindFirstServerByProfileId(string id);
        Task<List<Server>> GetServersByMemberId(string id);

        Task<Server?> FindServerByInviteCode(string inviteCode);
        Task<Server?> SaveMemberByInviteCode(string inviteCode, Member memberEntity);
    }
}
