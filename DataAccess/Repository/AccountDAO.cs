using BusinessObject.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountDAO
    {
        private readonly PRN231FamilyTreeContext _context;

        public AccountDAO(PRN231FamilyTreeContext context)
        {
            _context = context;
        }

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

        public async Task<Account> SaveCustomer(Account account)
        {
            var familyMember = await _context.FamilyMembers.FirstOrDefaultAsync(a => a.Email == account.Email);
            if (familyMember != null)
            {
                var accountNew = new Account
                {
                    FullName = account.FullName,
                    Email = account.Email,
                    Password = account.Password,
                    Role = 2,
                    MemberId = familyMember.Id,
                    //Member = familyMember
                };
                _context.Accounts.Add(accountNew);
                await _context.SaveChangesAsync();
                return accountNew;
            }
            return account;
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
