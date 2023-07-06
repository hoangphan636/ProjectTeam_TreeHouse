using BusinessObject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class FamilyTreeRequest
    {
        public int FamilyId { get; set; }
        public List<FamilyMember> Members { get; set; }
        public List<Relative> Relatives { get; set; }
    }
}
