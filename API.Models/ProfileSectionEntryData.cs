using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("profile_section_entry_data")]
    public class ProfileSectionEntryData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ProfileSectionEntryId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}