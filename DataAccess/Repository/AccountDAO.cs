using BusinessObject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountDAO
    {

        public static  List<Account> GetAccount()
        {
            var list = new List<Account>();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Accounts.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public static List<Account> SearchAccount(string Account)
        {
            var list = new List<Account>();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Accounts.Where(x => x.FullName.Contains(Account) || x.Email.Contains(Account)).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }


        public static Account GetAccountID(int ID)
        {
            var list = new Account();
            try
            {
                using (var context = new PRN231FamilyTreeContext())
                {
                    list = context.Accounts.FirstOrDefault(x => x.Id == ID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }






        public static void SaveCustomer(Account Accounts)
        {

            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Accounts.Add(Accounts);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateCustomer(Account Customer)
        {

            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Accounts.Update(Customer);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static void DeleteCustomer(Account Customer)
        {

            try
            {
                using var context = new PRN231FamilyTreeContext();

                context.Accounts.Remove(Customer);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }








    }
}
