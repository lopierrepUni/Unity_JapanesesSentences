using Assets.Scrips.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utilities
{
    public class ImageSentenceList
    {
        private static List<ImageSentence> ImageSentences = new List<ImageSentence>();

        public static void AddSentenceImage(ImageSentence imageSentence)
        {
            ImageSentences.Add(imageSentence);
        }
        public static ImageSentence GetSentenceImage()
        {

            ImageSentence imageSentence = ImageSentences.ElementAt(0);
            ImageSentences.RemoveAt(0);
            return imageSentence;
        }
        public static void ClearList()
        {
            ImageSentences.Clear();
        }
        public static int Count()
        {
            return ImageSentences.Count();
        }
        public static void Shuffle()
        {
            var rng = new System.Random();
            ImageSentences = ImageSentences.OrderBy(a => rng.Next()).ToList();
        }

    }
}
