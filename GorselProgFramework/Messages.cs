using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorselProgFramework
{
    public class Messages
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public string Channel { get; set; }
    }
}
