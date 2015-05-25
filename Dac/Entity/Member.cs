using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dac.Entity
{
    public class Member
    {
        public int NO { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string NAME { get; set; }
        public string PHONE { get; set; }
        public int LVL { get; set; }
        public string REG_ID { get; set; }
        public string REG_DATE { get; set; }
        public string EDIT_ID { get; set; }
        public string EDIT_DATE { get; set; }
    }
}