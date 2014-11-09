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
        private static string[] negationWords = {"never", "no", "nothing", "nowhere", "noone", "none", "not", "havent", "hasnt", "hadnt", "cant", "couldnt", "shouldnt", "wont", "wouldnt", "don't", "doesnt", "didnt", "isnt", "arent", "aint"};
        private static string[] punctuation = { ".", ":", ";", "!", "?", "...."};
        private static string negationIdentifier = "_NEG";

        public static string Tokenize(string reviewContent)
        {
            return negateHandler(reviewContent);
        }

        public static IEnumerable<string> GetAllWords(Review[] reviews)
        {
            foreach (var review in reviews)
                foreach (var w in review.Content.Split(' '))
                    yield return w;
        }

        private static IEnumerable<string> getReviews(Review[] reviews)
        {
            return reviews.Select(x => x.Content);
        }

        private static string negateHandler(string text)
        {
            string result = "";
            bool negate = false;
            foreach (var line in text.Split(punctuation, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] words = line.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                for(int i = 0; i< words.Length; i++)
                {
                    string s = words[i];
                    if (negationWords.Contains(words[i]))
                        negate = !negate;
                    if (negate)
                        words[i] += negationIdentifier;
                }
                result += " " + String.Join(" ", words);
            }
            return result;
        }

        private static string stripString(this string str)
        {
            foreach (var item in punctuation)
                str.Replace(item.ToString(), "");
            return str;
        }

        public static double Product(this IEnumerable<double> collection)
        {
            double res = 1;

            foreach (var item in collection)
                res *= item;

            return res;
        }

    }
}
