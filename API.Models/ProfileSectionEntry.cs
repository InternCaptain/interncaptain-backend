using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("profile_section_entries")]
    public class ProfileSectionEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ProfileSectionId { get; set; }
        public long Position { get; set; }


        public virtual ICollection<ProfileSectionEntryData> Data { get; set; }
    }
}