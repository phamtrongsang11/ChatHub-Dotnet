namespace TeamChat.Contracts.MessageContract
{
    public class UpdateMessageRequest
    {
        public string? id { get; set; }
        public string? content { get; set; }

        public string? fileUrl { get; set; }

        public Boolean? deleted { get; set; }

        public string? memberId { get; set; }

        public string? channelId { get; set; }
    }
}
