using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObject.DataAccess
{
    public partial class Family
    {
        public Family()
        {
            Activities = new HashSet<Activity>();
            Albums = new HashSet<Album>();
            FamilyMembers = new HashSet<FamilyMember>();
            Relatives = new HashSet<Relative>();
            StudyPromotions = new HashSet<StudyPromotion>();
        }

        public int Id { get; set; }
        public string FamilyName { get; set; }
        public int NumberOfMembers { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
        public virtual ICollection<Relative> Relatives { get; set; }
        public virtual ICollection<StudyPromotion> StudyPromotions { get; set; }
    }
}
