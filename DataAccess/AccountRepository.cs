using BusinessObject.DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{

    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDAO _accountDAO;

        public AccountRepository(AccountDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }
        public void DeleteCustomer(Account Customer) =>AccountDAO.DeleteCustomer(Customer);



        public List<Account> GetAccount() => AccountDAO.GetAccount();


        public Account GetAccountID(int ID) => AccountDAO.GetAccountID(ID);


        public async Task<Account> SaveCustomer(Account account) => await _accountDAO.SaveCustomer(account);


        public List<Account> SearchAccount(string Account) => AccountDAO.SearchAccount(Account);


        public void UpdateCustomer(Account Customer) => AccountDAO.UpdateCustomer(Customer);

    }
}
