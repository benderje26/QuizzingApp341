// Statistics is a model class that calculates the statistics of the scores from a live quiz
using QuizzingApp341;
using Supabase.Gotrue;

public class Statistics() {

    /// <summary>
    /// The scores of the quiz takers
    /// </summary>
    private List<int> Scores {
        get => _scores;
    }
    private List<int> _scores = [0];

    /// <summary>
    /// Sets the current session, or logs out.
    /// </summary>
    /// <param name="activeQuizId">The new current session or null if logging out</param>
    public async Task SetScores(long activeQuizId) {
        _scores = await MauiProgram.BusinessLogic.GetQuizScoresForActiveQuizId(activeQuizId) ?? [0];
    }

    // Get the min
    public double GetMin() {
        return Scores.Min();
    }

    // Get the max
    public double GetMax() {
        return Scores.Max();
    }

    // Get the median
    public double GetMedian() {
        Scores.Sort();
        // If there is an even number of scores
        if (Scores.Count % 2 == 0) {
            // Get the average of the two middle elements
            return ((Scores[Scores.Count / 2 - 1]) + (Scores[Scores.Count / 2])) / 2;

        } else { // If there is an odd number of scores
            // return the center element
            return Scores[Scores.Count / 2];
        }
    }

    public double GetLowerQuartile() {
        Scores.Sort(); // Sort
        return Scores[Scores.Count / 4]; // Get the first quarter element
    }

    public double GetUpperQuartile() {
        Scores.Sort(); // Sort
        return Scores[Scores.Count * 3 / 4]; // Get the third quarter element
    }
}