// Statistics is a model class that calculates the statistics of the scores from a live quiz
public class Statistics {
    List<int> scores = [55, 50, 75, 69, 79, 45, 88, 90, 25, 34, 10, 45, 40, 66, 78, 95, 14];

    // Get the min
    public double getMin() {
        return scores.Min();
    }

    // Get the max
    public double getMax() {
        return scores.Max();
    }

    // Get the median
    public double getMedian() {
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

    public double getLowerQuartile() {
        scores.Sort(); // Sort
        return scores[scores.Count / 4]; // Get the first quarter element
    }

    public double getUpperQuartile() {
        scores.Sort(); // Sort
        return scores[scores.Count * 3 / 4]; // Get the third quarter element
    }
}