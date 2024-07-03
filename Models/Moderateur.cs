using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas_The_True_Project.Models
{
    public class Moderateur
    {
        [Key]
        public int IdModerateur { get; set; }
        [Required(ErrorMessage = "Ce champ est requis")]
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
        [Required(ErrorMessage = "Ce champ est requis")]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Ne correspond pas au mot de passe")]
        public string ConfirmPassword { get; set; }
        public virtual ICollection<Report> Reports { get; set; }

    }
}
