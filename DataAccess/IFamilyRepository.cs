using BusinessObject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IFamilyRepository
    {
        List<Family> GetAllFamily();
        Family GetFamily(int id);
        List<Family> SearchFamily(string family);
        void Add(Family family);
        void Update(Family family);
        void Delete(Family family);
        IEnumerable<FamilyMember> GetAllFamilyMemberByFamily(int familyId);

    }
}
