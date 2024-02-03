using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TeamChat.Models
{
    public class Channel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string name { get; set; }

        [Required]
        public string type { get; set; } = ChannelType.TEXT;

        public string serverId { get; set; }
        public virtual Server? server { get; set; }

        public string profileId { get; set; }
        public virtual Profile? profile { get; set; }

        public virtual ICollection<Message>? messages { get; set; }
    }

    public class ChannelType
    {
        public static String TEXT
        {
            get { return "TEXT"; }
        }
        public static String AUDIOAUDIO
        {
            get { return "AUDIO"; }
        }
        public static String VIDEO
        {
            get { return "VIDEO"; }
        }
    }
}
