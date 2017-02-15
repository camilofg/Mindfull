using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIndFull
{
    public class Activity
    {
        //ActId, ActUserId, ActDesc, ActDate, ActState

        public int ActId { get; set; }
        public string ActUserId { get; set; }
        public string ActDesc { get; set; }
        public string ActDate { get; set; }
        public int ActState { get; set; }

        public Activity() { }
    }
}
