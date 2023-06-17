using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Activity
    {
        public int Id { get; set; }
        public int? FamilyId { get; set; }
        public string ActivityName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual Family Family { get; set; }
    }
}
