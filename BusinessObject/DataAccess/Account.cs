using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Account
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public int? MemberId { get; set; }
        [JsonIgnore]
        public virtual FamilyMember Member { get; set; }
    }
}
