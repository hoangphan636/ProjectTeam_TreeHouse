using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class FamilyDAO
    {
        public static List<Family> GetAllFamily()
        {
            var list = new List<Family>();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Families.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public static Family GetFamily(int id)
        {
            var list = new Family();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Families.FirstOrDefault(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public static List<Family> SearchFamily(string family)
        {
            var list = new List<Family>();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Families.Where(x => x.FamilyName.Contains(family)).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public static void Add(Family family)
        {
            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Families.Add(family);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the activity.", ex);
            }
        }

        public static void Update(Family family)
        {
            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Families.Update(family);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void Delete(Family family)
        {
            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Families.Remove(family);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static List<FamilyMember> GetAllFamilyMemberByFamily(int familyId)
        {
            using var _context = new PRN231FamilyTreeContext();

            var familyMembers = _context.FamilyMembers
                .Where(fm => fm.FamilyId == familyId)
                .ToList();

            return familyMembers;
        }
        public static List<FamilyMember> GetAllFamilyMemberByMemberId(int memberId)
        {
            using var context = new PRN231FamilyTreeContext();

            var member = context.FamilyMembers.FirstOrDefault(fm => fm.Id == memberId);
            if (member == null)
            {
                // Nếu không tìm thấy thành viên gia đình với memberId tương ứng, trả về danh sách rỗng
                return new List<FamilyMember>();
            }

            var familyMembers = context.FamilyMembers
                .Where(fm => fm.FamilyId == member.FamilyId)
                .ToList();

            return familyMembers;
        }

    }
}