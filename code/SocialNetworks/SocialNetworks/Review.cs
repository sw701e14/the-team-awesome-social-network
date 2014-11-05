using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public enum Score
    {
        Negative,
        Neutral,
        Positive
    }

    public class Review
    {
        public Review(string productId, string score, string summary, string text)
        {
            this.productId = productId;
            this.score = getScore(score);
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
            return new Review(review[0].Split(':')[1], review[4].Split(':')[1], review[6].Split(':')[1], review[7].Split(':')[1]);
        }

        private static Score getScore(string score)
        {
            switch (score)
            {
                case "1.0":
                case "2.0":
                    return Score.Negative;
                case "3.0":
                    return Score.Neutral;
                case "4.0":
                case "5.0":
                    return Score.Positive;
                default:
                    throw new ArgumentOutOfRangeException("The score has to be one of the following values: 1.0, 2.0, 3.0, 4.0, 5.0.");
            }
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

        private Score score;

        public Score Score
        {
            get { return score; }
        }

        private string content;

        public string Content
        {
            get { return Tokenizer.Tokenize(summary + " " + text); }
        }
        
    }
}
