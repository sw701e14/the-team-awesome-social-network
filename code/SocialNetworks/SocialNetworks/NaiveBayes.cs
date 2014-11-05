using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class NaiveBayes
    {
        private double NumberOfPositive;
        private double NumberOfNegative;
        private double NumberOfNeutral;
        private double NumberOfReviews;
        private double ProbabilityPositive;
        private double ProbabilityNegative;
        private double ProbabilityNeutral;
        private static double NUMBER_OF_CLASSES = 3; //Positive, Negative and Neutral
        private Dictionary<string, double> positiveProbabilities;
        private Dictionary<string, double> negativeProbabilities;
        private Dictionary<string, double> neutralProbabilities;
        private double scorePositive;
        private double scoreNegative;
        private double scoreNeutral;

        public NaiveBayes(Review[] reviews)
        {
            this.NumberOfPositive = reviews.Count(x => x.Score == Score.Positive);
            this.NumberOfNegative = reviews.Count(x => x.Score == Score.Negative);
            this.NumberOfNeutral = reviews.Count(x => x.Score == Score.Neutral);
            this.NumberOfReviews = reviews.Length;
            this.ProbabilityPositive = (this.NumberOfPositive + 1) / (this.NumberOfReviews + NUMBER_OF_CLASSES);
            this.ProbabilityNegative = (this.NumberOfNegative + 1) / (this.NumberOfReviews + NUMBER_OF_CLASSES);
            this.ProbabilityNeutral = (this.NumberOfNeutral + 1) / (this.NumberOfReviews + NUMBER_OF_CLASSES);
            this.positiveProbabilities = getProbabilities(reviews, Score.Positive);
            this.negativeProbabilities = getProbabilities(reviews, Score.Negative);
            this.neutralProbabilities = getProbabilities(reviews, Score.Neutral);

            this.scorePositive = this.positiveProbabilities.Values.Select(x => 1 - x).Product() * this.ProbabilityPositive;
            this.scoreNegative = this.negativeProbabilities.Values.Select(x => 1 - x).Product() * this.ProbabilityNegative;
            this.scoreNeutral = this.neutralProbabilities.Values.Select(x => 1 - x).Product() * this.ProbabilityNeutral;
        }

        public Score Evaluate(Review review)
        {
            double scoreReviewPositive = this.scorePositive * review.Content.Split(' ').Select(x => this.positiveProbabilities.ContainsKey(x) ? (this.positiveProbabilities[x] / (1 - this.positiveProbabilities[x])) : 1).Product();
            double scoreReviewNegative = this.scoreNegative * review.Content.Split(' ').Select(x => this.negativeProbabilities.ContainsKey(x) ? (this.negativeProbabilities[x] / (1 - this.negativeProbabilities[x])) : 1).Product();
            double scoreReviewNeutral = this.scoreNeutral * review.Content.Split(' ').Select(x => this.neutralProbabilities.ContainsKey(x) ? (this.neutralProbabilities[x] / (1 - this.neutralProbabilities[x])) : 1).Product();

            if (scoreReviewPositive > scoreReviewNegative && scoreReviewPositive > scoreReviewNeutral)
                return Score.Positive;
            else if (scoreReviewNegative > scoreReviewPositive && scoreReviewNegative > scoreReviewNeutral)
                return Score.Negative;
            else
                return Score.Neutral;
        }

        private Dictionary<string, double> getProbabilities(Review[] reviews, Score score)
        {
            Dictionary<string, double> probabilities = new Dictionary<string, double>();
            foreach (var word in Tokenizer.GetAllWords(reviews)) probabilities[word] = 1;
            foreach (var r in reviews)
            {
                if (!(r.Score == score))
                    continue;
                foreach (var word in r.Content.Split(' ')) //Måske distinct.. måske ikke..
                    probabilities[word]++;
            }
            switch (score)
            {
                case Score.Negative:
                    foreach (var k in probabilities.Keys.ToArray()) probabilities[k] /= (this.NumberOfNegative + Tokenizer.GetAllWords(reviews).Count());
                    break;
                case Score.Neutral:
                    foreach (var k in probabilities.Keys.ToArray()) probabilities[k] /= (this.NumberOfNeutral + Tokenizer.GetAllWords(reviews).Count());
                    break;
                case Score.Positive:
                    foreach (var k in probabilities.Keys.ToArray()) probabilities[k] /= (this.NumberOfPositive + Tokenizer.GetAllWords(reviews).Count());
                    break;
                default:
                    break;
            }
            
            return probabilities;
        }        
    }
}
