using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class FamilyMember
    {
        public FamilyMember()
        {
            Accounts = new HashSet<Account>();
            Relatives = new HashSet<Relative>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? FamilyId { get; set; }

        public virtual Family Family { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Relative> Relatives { get; set; }
    }
}
