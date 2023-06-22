using BusinessObject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ActivityDAO
    {
        public static List<Activity> GetAllActivity()
        {
            var list = new List<Activity>();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Activities.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public static Activity GetActivity(int id)
        {
            var list = new Activity();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Activities.FirstOrDefault(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public static List<Activity> SearchActivity(string activity)
        {
            var list = new List<Activity>();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Activities.Where(x => x.ActivityName.Contains(activity)).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public static void Add(Activity activity)
        {
            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Activities.Add(activity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the activity.", ex);
            }
        }

        public static void Update(Activity activity)
        {
            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Activities.Update(activity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void Delete(Activity activity)
        {
            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Activities.Remove(activity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
