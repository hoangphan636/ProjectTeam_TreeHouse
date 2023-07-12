using BusinessObject.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IImageRepository
    {
        Task<Image> Create(Image image);
        Task Delete(int id);
        Task<List<Image>> GetAllImage();
        Task<Image> GetImage(int id);
        Task<List<Image>> GetImageByAlbum(int id);
        public Task<Image> Update(int id, Image image);
    }
}