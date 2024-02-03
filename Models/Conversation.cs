using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChat.Models
{
    public class Conversation : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string memberOneId { get; set; }
        public virtual Member? memberOne { get; set; }

        [Required]
        public string memberTwoId { get; set; }
        public virtual Member? memberTwo { get; set; }

        public virtual ICollection<DirectMessage>? directMessages { get; set; }
    }
}
