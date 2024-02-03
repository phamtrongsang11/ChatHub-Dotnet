using TeamChat.Models;

namespace TeamChat.Contracts.DirectMessageContract
{
    public class DirectMessageResponse
    {
        public string id { get; set; }

        public string content { get; set; }

        public string fileUrl { get; set; }

        public Boolean deleted { get; set; }
        public virtual Member? member { get; set; }

        public virtual Conversation? conversation { get; set; }
        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
}
