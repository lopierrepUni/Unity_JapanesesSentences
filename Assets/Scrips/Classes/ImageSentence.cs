using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scrips.Classes
{
   public class ImageSentence
    {
        public int id{ get; set; }    
        public int Image_Id{ get; set; }
        public string Sentence { get; set; }

        public ImageSentence(int id, int image_Id, string sentence)
        {
            this.id = id;
            Image_Id = image_Id;
            Sentence = sentence;
        }
    }
}
