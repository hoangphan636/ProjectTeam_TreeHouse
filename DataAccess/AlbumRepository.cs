using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AlbumRepository : IRepository<Album>
    {
        private readonly PRN231FamilyTreeContext _context;

        public AlbumRepository(PRN231FamilyTreeContext context)
        {
            _context = context;
        }

        public Album Add(Album _object)
        {
            _context.Albums.Add(_object);
            return null;
        }

        public Album Find(int Id)
        {
            var album = _context.Albums.Find(Id);
            return album;
        }

        public IEnumerable<Album> List()
        {
            var list = _context.Albums.ToList();
            return list;
        }

        public int Update(int id, Album _object)
        {
            _context.Entry(_object).State = EntityState.Modified;
            return 1;
        }

        public int Delete(Album _object)
        {
            _context.Albums.Remove(_object);
            return 1;
        }

        public int SaveChanges()
        {
            var numOfChanges = _context.SaveChanges();
            return numOfChanges;
        }

        public bool Exists(int id)
        {
            var isExist = _context.Albums.Any(e => e.Id == id);
            return isExist;
        }

        public IEnumerable<Album> GetAlbumsByFamilyId(int familyId)
        {
            var list = _context.Albums.Where(x => x.FamilyId == familyId).ToList();
            return list;
        }
    }
}
