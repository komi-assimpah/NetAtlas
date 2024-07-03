using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas_The_True_Project.Models
{
    public class Publication
    {
        public Publication()
        {
            Ressources = new List<Ressource>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdPublication { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePublication { get; set; }

        //navigation properties
        public int IdMembre { get; set; }
        public Membre membre { get; set; }

        public virtual ICollection<Ressource> Ressources { get; set;}

    }
}