using TeamChat.Models;

namespace TeamChat.Contracts.ConversationContract
{
    public class ConversationResponse
    {
        public string id { get; set; }

        public virtual Member? memberOne { get; set; }
        public virtual Member? memberTwo { get; set; }
        public virtual ICollection<DirectMessage>? directMessages { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
