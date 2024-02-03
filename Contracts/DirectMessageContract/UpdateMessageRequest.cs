namespace TeamChat.Contracts.DirectMessageContract
{
    public class UpdateDirectMessageRequest
    {
        public string? id { get; set; }
        public string? content { get; set; }

        public string? fileUrl { get; set; }

        public Boolean? deleted { get; set; }

        public string? memberId { get; set; }

        public string? conversationId { get; set; }
    }
}
