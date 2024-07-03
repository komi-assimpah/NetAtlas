using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas_The_True_Project.Models
{
    public class Image_Video : Ressource
    {
        public double Taille { get; set; }
        [Required]
        [DisplayName("Description de votre publication")]
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string Type { get; set; }
        [NotMapped]
        [DisplayName("Fichier à charger")]
        public IFormFile Image_VideoFile { get; set; }

    }
}
