namespace TeamChat.Contracts.DirectMessageContract
{
    public class CreateDirectMessageRequest
    {
        public string content { get; set; }

        public string? fileUrl { get; set; }

        public string memberId { get; set; }

        public string conversationId { get; set; }
    }
}
