using TeamChat.Models;

namespace TeamChat.Contracts.ChannelContact
{
    public class ChannelResponse
    {
        public string id { get; set; }
        public string name { get; set; }

        public string type { get; set; }

        public virtual Server? server { get; set; }

        public virtual Profile? profile { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
