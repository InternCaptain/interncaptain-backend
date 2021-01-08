using API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("internships", Schema = "ic_internship")]
    public class Internship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public virtual Company Company { get; set; }
        
        public long RecruiterId { get; set; }
        public virtual User Recruiter { get; set; }
        
        public string PositionName { get; set; }
        public Domain Domain { get; set; }
        public string Description { get; set; }
    }
}
