using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RelationshipRepository : IRepository<Relationship>
    {
        private readonly PRN231FamilyTreeContext _context;

        public RelationshipRepository(PRN231FamilyTreeContext context)
        {
            _context = context;
        }

        public Relationship Add(Relationship _object)
        {
            _context.Relationships.Add(_object);
            return null;
        }

        public Relationship Find(int Id)
        {
            var relationship = _context.Relationships.Find(Id);
            return relationship;
        }

        public IEnumerable<Relationship> List()
        {
            var list = _context.Relationships.ToList();
            return list;
        }

        public int Update(int id, Relationship _object)
        {
            _context.Entry(_object).State = EntityState.Modified;
            return 1;
        }

        public int Delete(Relationship _object)
        {
            _context.Relationships.Remove(_object);
            return 1;
        }

        public int SaveChanges()
        {
            var numOfChanges = _context.SaveChanges();
            return numOfChanges;
        }

        public bool Exists(int id)
        {
            var isExist = _context.Relationships.Any(e => e.Id == id);
            return isExist;
        }
    }
}
