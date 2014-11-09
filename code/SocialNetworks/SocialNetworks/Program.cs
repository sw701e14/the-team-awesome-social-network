using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Review> reviews = new List<Review>();
            int count = 0;
            foreach (var item in Review.ParseFile("../../../../_ignoreSentimentTrainingData.txt").Take(10000))
            {
                count++;
                Console.WriteLine(count);
                reviews.Add(item);
            }

            NaiveBayes nb = new NaiveBayes(reviews.ToArray());
        }
    }
}
