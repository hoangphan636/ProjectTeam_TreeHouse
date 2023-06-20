using BusinessObject;
using BusinessObject.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ILoginRepository
    {
        Tokens Authenticate(Login users);
    }
}
