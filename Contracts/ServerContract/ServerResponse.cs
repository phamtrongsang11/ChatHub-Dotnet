using TeamChat.Models;

namespace TeamChat.Contracts.ServerContract
{
    public class ServerRespone
    {
        public string id { get; set; }

        public string name { get; set; }

        public string imageUrl { get; set; }

        public string inviteCode { get; set; }

        public Profile? profile { get; set; }
        public ICollection<Member>? members { get; set; }
        public ICollection<Channel>? channels { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
