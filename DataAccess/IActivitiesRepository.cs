using BusinessObject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IActivitiesRepository
    {
        List<Activity> GetAllActivity();
        Activity GetActivity(int id);
        List<Activity> SearchActivity(string activity);
        void Add(Activity activity);
        void Update(Activity activity);
        void Delete(Activity activity);
        List<Activity> GetActivitiesByFamilyId(int familyId);

    }
}
