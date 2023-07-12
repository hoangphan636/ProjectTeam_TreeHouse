using BusinessObject.DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageDAO _imageDAO;

        public ImageRepository(ImageDAO imageDAO)
        {
            _imageDAO = imageDAO;
        }
        public async Task<List<Image>> GetAllImage()
        {
            return await _imageDAO.GetAll();
        }

        public async Task<Image> GetImage(int id) => await _imageDAO.GetImage(id);
        public async Task<Image> Create(Image image) => await _imageDAO.Create(image);
        public async Task<Image> Update(int id,Image image) => await _imageDAO.Update(id,image);
        public async Task Delete(int id) => await _imageDAO.Delete(id);

        public async Task<List<Image>>GetImageByAlbum(int id)=> await _imageDAO.GetImageByAlbum(id);
        
    }
}
