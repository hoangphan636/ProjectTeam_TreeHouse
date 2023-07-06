using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Tokens
    {
        [Key]
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
