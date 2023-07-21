using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Activity
    {
        public int Id { get; set; }
        [Required]
        public int? FamilyId { get; set; }
        [Required]
        public string ActivityName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Family Family { get; set; }
    }
}