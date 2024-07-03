using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NetAtlas_The_True_Project.Models
{
    public class Membre
    {
        public Membre()
        {
            Friends = new List<AmiMembre>();
            Publications = new List<Publication>();
        }

        [Key]
        public int IdMembre { get; set; }
        [Required(ErrorMessage ="Ce champ est requis")]
        public string Nom { get; set; }
        [Required(ErrorMessage = "Ce champ est requis")]
        public string Prenom { get; set; }
        [Required(ErrorMessage = "Ce champ est requis")]
        public string? AdresseMail { get; set; }
        [Required(ErrorMessage = "Ce champ est requis")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ce champ est requis")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Le mot de passe doit être supérieur à 5 caractères")]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage ="Ne correspond pas au mot de passe")]
        public string ConfirmPassword { get; set; }
        public int Report { get; set; } = 0;
        //Ami membre
        [JsonIgnore]
        public virtual ICollection<AmiMembre> SentFriendRequests { get; set; }
        [JsonIgnore]
        public virtual ICollection<AmiMembre> ReceievedFriendRequests { get; set; }

        [NotMapped]
        public virtual ICollection<AmiMembre> Friends { get; set; }
        
        //Navigation properties
        public virtual ICollection<Publication> Publications { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        

    }
}
