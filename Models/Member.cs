using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TeamChat.Models
{
    public class Member : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string role { get; set; } = MemberRole.GUEST;

        public string serverId { get; set; }
        public virtual Server? server { get; set; }

        public string profileId { get; set; }
        public virtual Profile? profile { get; set; }

        public virtual ICollection<Message>? messages { get; set; }
        public virtual ICollection<DirectMessage>? directMessages { get; set; }
        public virtual ICollection<Conversation>? conversationsInitialted { get; set; }
        public virtual ICollection<Conversation>? conversationsReceived { get; set; }
    }

    public class MemberRole
    {
        public static String ADMIN
        {
            get { return "ADMIN"; }
        }
        public static String MODERATOR
        {
            get { return "MODERATOR"; }
        }
        public static String GUEST
        {
            get { return "GUEST"; }
        }
    }
}
