using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TeamChat.Models
{
    public class Server : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string name { get; set; }

        [Required]
        public string imageUrl { get; set; }

        [Required]
        public string inviteCode { get; set; }

        public string profileId { get; set; }

        public virtual Profile? profile { get; set; }
        public virtual ICollection<Member>? members { get; set; }
        public virtual ICollection<Channel>? channels { get; set; }
    }
}
