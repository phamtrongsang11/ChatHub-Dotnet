namespace TeamChat.Contracts.ServerContract
{
    public class UpdateServerRequest
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? imageUrl { get; set; }
        public string? inviteCode { get; set; }
        public string? profileId { get; set; }
    }
}
