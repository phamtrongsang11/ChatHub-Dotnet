namespace TeamChat.Contracts.ConversationContract
{
    public class UpdateConversationRequest
    {
        public string? id { get; set; }
        public string? memberOneId { get; set; }
        public string? memberTwoId { get; set; }
    }
}
