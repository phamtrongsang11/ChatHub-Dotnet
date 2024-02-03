namespace TeamChat.Contracts.ServerContract
{
    public class CreateServerRequest
    {
        public string name { get; set; }

        public string imageUrl { get; set; }

        public string inviteCode { get; set; }

        public string profileId { get; set; }
    }
}
