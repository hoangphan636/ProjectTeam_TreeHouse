using BusinessObject.DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FamilyRepository : IFamilyRepository
    {
        public List<Family> GetAllFamily() => FamilyDAO.GetAllFamily();
        public Family GetFamily(int id) => FamilyDAO.GetFamily(id);
        public List<Family> SearchFamily(string family) => FamilyDAO.SearchFamily(family);
        public void Add(Family family) => FamilyDAO.Add(family);
        public void Update(Family family) => FamilyDAO.Update(family);
        public void Delete(Family family) => FamilyDAO.Delete(family);
    }
}
