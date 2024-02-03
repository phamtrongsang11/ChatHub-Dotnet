using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChat.Models
{
    public class BaseModel
    {
        public DateTime? createdAt { get; set; }

        public DateTime? updatedAt { get; set; }
    }
}
