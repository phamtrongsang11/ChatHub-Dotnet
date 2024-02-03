using Models = TeamChat.Models;

namespace TeamChat.Contracts.ProfileContract
{
    public class ProfileResponse
    {
        public string id { get; set; }

        public string name { get; set; }

        public string? imageUrl { get; set; }

        public string email { get; set; }

        public ICollection<Models.Server>? servers { get; set; }
        public ICollection<Models.Member>? members { get; set; }
        public ICollection<Models.Channel>? channels { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
