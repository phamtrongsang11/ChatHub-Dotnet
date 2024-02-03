using System.Linq.Expressions;
using TeamChat.Models;

namespace TeamChat.Services.MemberService
{
    public interface IMemberService
    {
        Task<Member> Create(Member memberEntity);
        Task<List<Member>> GetAll(string? includeProperties = null);

        Task<Member?> Get(Expression<Func<Member, bool>> filter, string? includeProperties = null);

        Task<Member> Update(Member memberEntity);

        Task<Member> PartialUpdate(string id, Member memberEntity);

        Task<Boolean> isExist(string id);
        Task<Boolean> checkIsExistByReference(string serverId, string profileId);

        Task<Boolean> Remove(Member memberEntity);
    }
}
