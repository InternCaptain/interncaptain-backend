using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("profile_section_fields")]
    public class ProfileSectionField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ProfileSectionId { get; set; }

        public string Name { get; set; }
        public ProfileSectionFieldKind Kind { get; set; }
    }
}