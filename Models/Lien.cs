using System.ComponentModel.DataAnnotations;

namespace NetAtlas_The_True_Project.Models
{
    public class Lien : Ressource
    {
        [Required]
        public string AdresseUrl { get; set; }
    }
}
