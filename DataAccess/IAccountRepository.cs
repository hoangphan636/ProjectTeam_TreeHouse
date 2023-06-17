using BusinessObject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IAccountRepository
    {
         List<Account> GetAccount();
        List<Account> SearchAccount(string Account);
        Account GetAccountID(int ID);
        void SaveCustomer(Account Accounts);
        void UpdateCustomer(Account Customer);
        void DeleteCustomer(Account Customer);



    }
}
