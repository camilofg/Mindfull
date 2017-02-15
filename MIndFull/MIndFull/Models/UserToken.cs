using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIndFull.Models
{
    public class UserToken
    {
        public UserToken() { }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UsID { get; set; }
        public string UsToken { get; set; }
    }

    public class MenuOptions {
        public MenuOptions() { }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public bool IsHeader { get; set; }
    }
}
