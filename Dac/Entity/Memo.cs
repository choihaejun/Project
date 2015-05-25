using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dac.Entity
{
    public class Memo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
        public string Del_Flag { get; set; }
        public string Important { get; set; }
        public ImageFile[] Images { get; set; }
        public int Reg_ID { get; set; }
        public DateTime Reg_Date { get; set; }
        public int Edit_ID { get; set; }
        public string Edit_Date { get; set; }

    }
}