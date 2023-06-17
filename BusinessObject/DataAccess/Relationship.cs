using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Relationship
    {
        public Relationship()
        {
            Relatives = new HashSet<Relative>();
        }

        public int Id { get; set; }
        public string RelationType { get; set; }

        public virtual ICollection<Relative> Relatives { get; set; }
    }
}
