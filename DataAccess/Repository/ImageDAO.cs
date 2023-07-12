using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ImageDAO
    {
        private readonly PRN231FamilyTreeContext _context;

        public ImageDAO(PRN231FamilyTreeContext context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetAll()
        {
            return await _context.Images.ToListAsync();
        }
        public async Task<Image>GetImage(int id)
        {
            return await _context.Images.FindAsync(id);
        }
        public async Task<Image>Create(Image image)
        {
            var imageNew = await _context.Images.FindAsync(image.Id);
            if (imageNew == null)
            {
                _context.Images.Add(image);
                image.CreateDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return image;
        }

        public async Task<Image>Update(int id,Image image)
        {
            var imageUpdate = await _context.Images.FindAsync(id);
            if (imageUpdate != null)
            {
                _context.Images.Update(image);
                await _context.SaveChangesAsync();
            }
            return image;
        }

        public async Task Delete (int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Image>>GetImageByAlbum(int id)
        {
            return await _context.Images.Where(m => m.AlbumId == id).ToListAsync();
        }
    }
}
