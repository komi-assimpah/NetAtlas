using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetAtlas_The_True_Project.Models
{
    public class Message : Ressource
    {
        [Required]
        [DisplayName("Votre message")]
        public string MessageSend { get; set; }
    }
}
