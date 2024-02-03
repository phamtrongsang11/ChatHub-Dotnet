using TeamChat.Models;

namespace TeamChat.Contracts.MessageContract
{
    public class MessageResponse
    {
        public string id { get; set; }

        public string content { get; set; }

        public string fileUrl { get; set; }

        public Boolean deleted { get; set; }

        public virtual Member? member { get; set; }

        public virtual Channel? channel { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
}
