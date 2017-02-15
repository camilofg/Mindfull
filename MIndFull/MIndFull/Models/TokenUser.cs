using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIndFull.Models
{
    public class TokenUser
    {
        public User SessionUser { get; set; }
        public string ValidationToken { get; set; }
        public string Msg { get; set; }
    }
}
