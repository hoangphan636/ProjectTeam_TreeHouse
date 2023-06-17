using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Relative
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int? RelationId { get; set; }
        public int MemberRelativeId { get; set; }

        public virtual FamilyMember Member { get; set; }
        public virtual Relationship Relation { get; set; }
    }
}
