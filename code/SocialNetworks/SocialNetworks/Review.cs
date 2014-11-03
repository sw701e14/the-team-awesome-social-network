using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class Review
    {
        public Review(string productId, double score, string summary, string text)
        {
            this.productId = productId;
            this.score = score;
            this.text = text;
            this.summary = summary;
        }

        public static IEnumerable<Review> ParseFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            string[] review = new string[8];
            int reviewCounter = 0;
            for (int i = 0; i < lines.Count(); i++)
            {                
                if (lines[i] == "")
                {
                    reviewCounter=0;
                    yield return createReview(review);
                    continue;
                }
                review[reviewCounter] = lines[i];                
                reviewCounter++;       
            }
        }

        private static Review createReview(string[] review)
        {
            return new Review(review[0].Split(':')[1], double.Parse(review[4].Split(':')[1].Trim(),System.Globalization.CultureInfo.InvariantCulture), review[6].Split(':')[1], review[7].Split(':')[1]);
        }        

        private string productId;

        public string ProductId
        {
            get { return productId; }
        }

        private string text;

        public string Text
        {
            get { return text; }
        }

        private string summary;

        public string Summary
        {
            get { return summary; }
        }

        private double score;

        public double Score
        {
            get { return score; }
        }

        private string content;

        public string Content
        {
            get { return summary + " " + text; }
        }
        
    }
}
