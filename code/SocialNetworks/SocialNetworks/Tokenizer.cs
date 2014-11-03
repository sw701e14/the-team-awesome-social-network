using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public static class Tokenizer
    {
        //Taken from slides
        private static string[] negationWords = {"never", "no", "nothing", "nowhere", "noone", "none", "not", "havent", "hasnt", "hadnt", "cant", "couldnt", "shouldnt", "wont", "wouldnt", "dont", "doesnt", "didnt", "isnt", "arent", "aint"};
        private static char[] punctuation = { '.', ':', ';', '!', '?' };
        private static char[] removals = { '.', ':', ';', '!', '?' };
        private static string negationIdentifier = "_NEG";

        public string[] Tokenize(Review[] reviews)
        {
            string[] r = getReviews(reviews).ToArray();
            throw new NotImplementedException();
        }

        public static IEnumerable<string> GetAllWords(Review[] reviews)
        {
            foreach (var review in reviews)
                foreach (var w in (review.Summary + review.Text).Split(' '))
                {
                    yield return w;
                    yield return w + "_NEG";
                }
        }

        private static IEnumerable<string> getReviews(Review[] reviews)
        {
            foreach (var r in reviews)
                yield return r.Content;
        }

        private static string stripString(this string text)
        {
            foreach (var removalChar in removals)
                text.Replace(removalChar.ToString(), "");
            return text;
        }

        private string negateHandler(string text)
        {
            string result = "";
            bool negate = false;
            foreach (var line in text.stripString().Split(punctuation))
            {
                string[] words = line.Split(' ');
                for(int i = 0; i< words.Length; i++)
                {                    
                    if (negationWords.Contains(words[i]))
                        negate = !negate;
                    if (negate)
                        words[i] += negationIdentifier;
                }
                result += " " + words.ToString();
            }
            return result;
        }
    }
}
