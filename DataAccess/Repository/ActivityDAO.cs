﻿using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
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

        public static void Update(int id, Activity activity)
        {
            try
            {
                using var context = new PRN231FamilyTreeContext();
                var existingActivity = context.Activities.FirstOrDefault(x => x.Id == id);
                if (existingActivity != null)
                {
                    context.Entry(existingActivity).CurrentValues.SetValues(activity);
                    context.SaveChanges();
                }
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

        public static List<Activity> GetActivitiesByFamilyId(int familyId)
        {
            using var dbContext = new PRN231FamilyTreeContext();

            return dbContext.Activities
                .Where(a => a.FamilyId == familyId)
                .ToList();
        }
        public static List<Activity> GetActivitiesByMemberId(int memberId)
        {
            using var dbContext = new PRN231FamilyTreeContext();

            // Tìm familyId dựa trên memberId
            var familyId = dbContext.FamilyMembers
                .Where(fm => fm.Id == memberId)
                .Select(fm => fm.FamilyId)
                .FirstOrDefault();

            if (familyId == null)
            {
                throw new Exception("Invalid memberId");
            }

            // Lấy danh sách activities của family đó
            return dbContext.Activities
                .Where(a => a.FamilyId == familyId)
                .ToList();
        }


    }
}