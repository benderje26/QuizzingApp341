namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

public partial class QuizHistory : ContentPage {
    private readonly IBusinessLogic _businessLogic;

    public ObservableCollection<History> Quizzes { get; set; }

    public QuizHistory() {
        InitializeComponent();
        Quizzes = new ObservableCollection<History>();
        BindingContext = this;

        LoadActiveQuizIdFromParticipants();
    }

    private async void LoadActiveQuizIdFromParticipants() {
        try {
            var activeQuizIds = await MauiProgram.BusinessLogic.GetActiveQuizIdsForUser();
            await LoadQuizIdFromActiveQuiz(activeQuizIds);
        } catch (Exception ex) {
            await DisplayAlert("Error", $"Failed to load active_quiz_id: {ex.Message}", "OK");
        }
    }

    private async Task LoadQuizIdFromActiveQuiz(List<long> activeQuizIds) {
        try {
            // Fetch QuizId and StartTime
            List<ActiveQuiz> quizIdAndTime = await MauiProgram.BusinessLogic.GetActiveQuizzesByActiveQuizIds(activeQuizIds);

            // Check if data was found
            if (quizIdAndTime.Count > 0) {
                // Sort the quizzes by StartTime from most recent to oldest
                var sortedQuizList = quizIdAndTime.OrderByDescending(q => q.StartTime ?? DateTime.MinValue).ToList();

                Quizzes.Clear();

                foreach (ActiveQuiz activeQuiz in sortedQuizList) {
                    // Create a new History object for each quiz - storing QuizId and StartTime and quizTitle
                    Quizzes.Add(new History(activeQuiz.QuizId, activeQuiz.StartTime, activeQuiz.QuizTitle ?? string.Empty));
                }
            } else {
                await DisplayAlert("No Quizzes", "No quizzes found for the active quizzes.", "OK");
            }

        } catch (Exception ex) {
            await DisplayAlert("Error", $"Failed to load QuizId: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// History class to store quiz information
    /// </summary>
    public class History(long quizId, DateTime? startTime, string quizTitle) {
        public DateTime? StartTime { get; set; } = startTime;
        public long QuizId { get; set; } = quizId;
        public string QuizTitle { get; set; } = quizTitle;
    }
}
