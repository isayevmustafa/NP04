using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace np04CA1
{
    internal class Command
    {
        public const string ProcessList = "PROCLIST";
        public const string Kill = "KILL";
        public const string RUN = "RUN";

        public string? Text { get; set; }
        public string? Param { get; set; }




    }
}
