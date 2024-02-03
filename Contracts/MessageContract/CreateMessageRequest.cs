namespace TeamChat.Contracts.MessageContract
{
    public class CreateMessageRequest
    {
        public string content { get; set; }

        public string? fileUrl { get; set; }

        public string memberId { get; set; }

        public string channelId { get; set; }
    }
}
