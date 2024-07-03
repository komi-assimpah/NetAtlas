using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas_The_True_Project.Models
{
    public class Report
    {
        [Key]
        public int IdReport { get; set; }
        public int IdMembre { get; set; }
        public int IdModerateur { get; set; }
        public string MessageAvertissement { get; set; } = "Votre publication n'est pas conforme à nos conditions d'utilisation";
        public Moderateur Moderateur { get; set; }
        public Membre Membre { get; set; }
    }
}
