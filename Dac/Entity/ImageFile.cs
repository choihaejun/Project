using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dac.Entity
{
    public class ImageFile
    {
        public int ID { get; set; }
        public string SaveName { get; set; }
        public string OriginalName { get; set; }
        public string Src { get; set; }
        public string Reg_ID { get; set; }
        public string Reg_Date { get; set; }
    }
}