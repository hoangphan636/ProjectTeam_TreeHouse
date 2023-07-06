using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RelativeRepository : IRepository<Relative>
    {
        private readonly PRN231FamilyTreeContext _context;

        public RelativeRepository(PRN231FamilyTreeContext context)
        {
            _context = context;
        }

        public Relative Add(Relative _object)
        {
            _context.Relatives.Add(_object);
            return null;
        }

        public Relative Find(int Id)
        {
            var relative = _context.Relatives.Find(Id);
            return relative;
        }

        public IEnumerable<Relative> List()
        {
            var list = _context.Relatives.ToList();
            return list;
        }

        public int Update(int id, Relative _object)
        {
            _context.Entry(_object).State = EntityState.Modified;
            return 1;
        }

        public int Delete(Relative _object)
        {
            _context.Relatives.Remove(_object);
            return 1;
        }

        public int SaveChanges()
        {
            var numOfChanges = _context.SaveChanges();
            return numOfChanges;
        }

        public bool Exists(int id)
        {
            var isExist = _context.Relatives.Any(e => e.Id == id);
            return isExist;
        }

        public IEnumerable<Relative> GetRelativesByMemberId(int memberId)
        {
            return _context.Relatives.Where(x => x.MemberId == memberId).ToList();
        }

        public IEnumerable<Relative> GetRelativesByRelationWithMemberId(int memberId, int relationId)
        {
            return _context.Relatives.Where(x => x.MemberId == memberId && x.RelationId == relationId).ToList();
        }
    }
}
