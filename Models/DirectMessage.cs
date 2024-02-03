using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChat.Models
{
    public class DirectMessage : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string content { get; set; }

        public string? fileUrl { get; set; } = null;

        public Boolean deleted { get; set; }

        public string memberId { get; set; }
        public virtual Member? member { get; set; }

        public string conversationId { get; set; }
        public virtual Conversation? conversation { get; set; }
    }
}
