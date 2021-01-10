using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("profile_sections")]
    public class ProfileSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ProfileId { get; set; }

        public string Name { get; set; }
        public long Position { get; set; }

        public virtual ICollection<ProfileSectionField> Fields { get; set; }
        public virtual ICollection<ProfileSectionEntry> Entries { get; set; }
    }
}