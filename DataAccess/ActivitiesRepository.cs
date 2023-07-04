using BusinessObject.DataAccess;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ActivitiesRepository : IActivitiesRepository
    {
        public List<Activity> GetAllActivity() => ActivityDAO.GetAllActivity();
        public Activity GetActivity(int id) => ActivityDAO.GetActivity(id);
        public List<Activity> SearchActivity(string activity) => ActivityDAO.SearchActivity(activity);
        public void Add(Activity activity) => ActivityDAO.Add(activity);
        public void Update(Activity activity) => ActivityDAO.Update(activity);
        public void Delete(Activity activity) => ActivityDAO.Delete(activity);
        public List<Activity> GetActivitiesByFamilyId(int familyId) => ActivityDAO.GetActivitiesByFamilyId(familyId);
        public List<Activity> GetActivitiesByMemberId(int memberId) => ActivityDAO.GetActivitiesByMemberId(memberId);
    }
}
