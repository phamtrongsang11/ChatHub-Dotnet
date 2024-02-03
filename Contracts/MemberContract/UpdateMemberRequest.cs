using TeamChat.Models;

namespace TeamChat.Contracts.MemberContract
{
    public class UpdateMemberRequest
    {
        public string? id { get; set; }

        public string? role { get; set; }

        public string? serverId { get; set; }

        public string? profileId { get; set; }
    }
}
