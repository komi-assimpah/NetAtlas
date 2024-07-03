using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetAtlas_The_True_Project.Models
{
    public abstract class Ressource
    {
        [Key]
        public int IdRessource { get; set; }
        //navigation properties
        public int IdPublication { get; set; }
        public isReport IsThisReported { get; set; }=isReport.None;
        public Publication Publication { get; set; }
    }
    public enum isReport
    {
        None,
        isReport
    }
}
