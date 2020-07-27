using Assets.Scrips.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scrips.Classes
{
    public class Word
    {
        public int id { get; set; }
        public string word { get; set; }
        public WordCategory Category { get; set; }
        public int Leve { get; set; }
    }
}
