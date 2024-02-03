using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace TeamChat.Models
{
    public class Profile : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string name { get; set; }

        public string? imageUrl { get; set; } = null;

        [Required]
        public string email { get; set; }

        public virtual ICollection<Server>? servers { get; set; }
        public virtual ICollection<Member>? members { get; set; }
        public virtual ICollection<Channel>? channels { get; set; }
    }
}
