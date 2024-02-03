using TeamChat.Models;

namespace TeamChat.Contracts.MemberContract
{
    public class MemberResponse
    {
        public string id { get; set; }

        public string role { get; set; }

        public virtual Server? server { get; set; }

        public virtual Profile? profile { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
