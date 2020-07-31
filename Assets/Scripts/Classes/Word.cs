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
        public string stringword { get; set; }
        public WordCategory Category { get; set; }
        public int Level { get; set; }

        public Word(int id, string word, WordCategory category, int level)
        {
            this.id = id;
            this.stringword = word;
            Category = category;
            Level = level;
        }
    }
}
