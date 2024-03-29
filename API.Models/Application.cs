﻿using System;
using API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("applications")]
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public long InternshipId { get; set; }
        public virtual Internship Internship { get; set; }
        
        public long StudentId { get; set; }
        public virtual User Student { get; set; }
        
        public ApplicationStatus Status { get; set; }
    }
}
