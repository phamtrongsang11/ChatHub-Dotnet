using TeamChat.Models;

namespace TeamChat.Contracts.ChannelContact
{
    public class UpdateChannelRequest
    {
        public string? id { get; set; }
        public string? name { get; set; }

        public string? type { get; set; }

        public string? serverId { get; set; }

        public string? profileId { get; set; }
    }
}
