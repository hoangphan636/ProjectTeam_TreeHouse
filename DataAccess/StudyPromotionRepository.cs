using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StudyPromotionRepository : IRepository<StudyPromotion>
    {
        private readonly PRN231FamilyTreeContext _context;

        public StudyPromotionRepository(PRN231FamilyTreeContext context)
        {
            _context = context;
        }

        public StudyPromotion Add(StudyPromotion _object)
        {
            _context.StudyPromotions.Add(_object);
            return null;
        }

        public StudyPromotion Find(int Id)
        {
            var album = _context.StudyPromotions.Find(Id);
            return album;
        }

        public IEnumerable<StudyPromotion> List()
        {
            var list = _context.StudyPromotions.ToList();
            return list;
        }

        public int Update(int id, StudyPromotion _object)
        {
            _context.Entry(_object).State = EntityState.Modified;
            return 1;
        }

        public int Delete(StudyPromotion _object)
        {
            _context.StudyPromotions.Remove(_object);
            return 1;
        }

        public int SaveChanges()
        {
            var numOfChanges = _context.SaveChanges();
            return numOfChanges;
        }

        public bool Exists(int id)
        {
            var isExist = _context.StudyPromotions.Any(e => e.Id == id);
            return isExist;
        }

        public IEnumerable<StudyPromotion> GetStudyPromotionsByFamilyId(int familyId)
        {
            var list = _context.StudyPromotions.Where(x => x.FamilyId == familyId).ToList();
            return list;
        }
    }
}
