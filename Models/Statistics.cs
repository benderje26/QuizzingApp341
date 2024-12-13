// Statistics is a model class that calculates the statistics of the scores from a live quiz
public class Statistics(List<int> scores) {

    public List<int>? Scores { get { return scores; } }
    public double Minimum { get { return GetMin(scores); } }
    public double Maximum { get { return GetMax(scores); } }
    public double Mean { get { return ((int)(GetMean(scores) * 100)) / 100d; } }
    public double Median { get { return GetMedian(scores); } }
    public double LowerQuartile { get { return GetLowerQuartile(scores); } }
    public double UpperQuartile { get { return GetUpperQuartile(scores); } }

    // Get the min
    public double GetMin(List<int> scores) {
        return scores.Min();
    }

    // Get the max
    public double GetMax(List<int> scores) {
        return scores.Max();
    }

    // Get the median
    public double GetMedian(List<int> scores) {
        scores.Sort();
        // If there is an even number of scores
        if (scores.Count % 2 == 0) {
            // Get the average of the two middle elements
            return ((scores[scores.Count / 2 - 1]) + (scores[scores.Count / 2])) / 2;

        } else { // If there is an odd number of scores
            // return the center element
            return scores[scores.Count / 2];
        }
    }

    public double GetMean(List<int> scores) {
        return scores.Average();
    }

    public double GetLowerQuartile(List<int> scores) {
        scores.Sort(); // Sort
        return scores[scores.Count / 4]; // Get the first quarter element
    }

    public double GetUpperQuartile(List<int> scores) {
        scores.Sort(); // Sort
        return scores[scores.Count * 3 / 4]; // Get the third quarter element
    }
}