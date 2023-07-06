using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string FamilyName { get; set; }
    }
}
