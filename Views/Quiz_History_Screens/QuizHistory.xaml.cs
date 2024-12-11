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
                    Quizzes.Add(new History(activeQuiz.QuizId, activeQuiz.StartTime, activeQuiz.QuizTitle ?? string.Empty, activeQuiz.Id));
                }
            } else {
                await DisplayAlert("No Quizzes", "No quizzes found for the active quizzes.", "OK");
            }

        } catch (Exception ex) {
            await DisplayAlert("Error", $"Failed to load QuizId: {ex.Message}", "OK");
        }
    }

    private async void OnStudyButtonClicked(object sender, EventArgs e) {
        var button = sender as Button;
        var activeQuizId = button?.CommandParameter as long?;

        if (activeQuizId.HasValue) {
            var quizTitle = Quizzes.FirstOrDefault(q => q.ActiveQuizId == activeQuizId.Value)?.QuizTitle;
            await DisplayAlert("Study", $"Start studying {quizTitle}?", "OK");
        }
    }


    private async void OnViewStatsButtonClicked(object sender, EventArgs e) {
        var button = sender as Button;
        // Retrieve the CommandParameter
        var historyItem = button?.CommandParameter as History;

        // Check if the historyItem is not null
        if (historyItem != null) {
            // Display an alert to the user to confirm that they want to view the quiz statistics
            await DisplayAlert("View", $"Show Quiz {historyItem.QuizTitle} statistics?", "OK");

            Console.WriteLine($"View Stats button clicked for quiz: {historyItem.QuizTitle}");

            // Create a new StatisticsScreen instance (this is your destination page)
            var statisticsScreen = new StatisticsScreen();

            // pass the quiz title and active quiz ID
            statisticsScreen.BindingContext = new { QuizTitle = historyItem.QuizTitle, ActiveQuizId = historyItem.ActiveQuizId };

            // Navigate to the StatisticsScreen page
            await Navigation.PushAsync(statisticsScreen);
        }
    }


    private async void OnDeleteButtonClicked(object sender, EventArgs e) {
        var button = sender as Button;
        var activeQuizId = button?.CommandParameter as long?;

        if (activeQuizId.HasValue) {
            bool success = await DeleteQuizFromHistory(activeQuizId.Value);

            if (success) {
                //remove the quiz from the Quizzes collection 
                Quizzes.Remove(Quizzes.FirstOrDefault(q => q.ActiveQuizId == activeQuizId.Value));
                await DisplayAlert("Deleted", $"Quiz '{activeQuizId}' has been deleted.", "OK");
            } else {
                await DisplayAlert("Error", $"Failed to delete quiz '{activeQuizId}'.", "OK");
            }
        } else {
            await DisplayAlert("Error", "Invalid Quiz ID.", "OK");
        }
    }

    public async Task<bool> DeleteQuizFromHistory(long activeQuizId) {
        try {
            Console.WriteLine($"Attempting to delete quiz: {activeQuizId}");

            bool quizDeleted = await MauiProgram.BusinessLogic.DeleteQuizFromHistory(activeQuizId);

            return quizDeleted;
        } catch (Exception ex) {
            Console.WriteLine($"Error while deleting quiz '{activeQuizId}': {ex.Message}\n{ex.StackTrace}");
            return false;
        }
    }



    /// <summary>
    /// History class to store quiz information
    /// </summary>
    public class History(long quizId, DateTime? startTime, string quizTitle, long Id) {
        public DateTime? StartTime { get; set; } = startTime;
        public long QuizId { get; set; } = quizId;
        public string QuizTitle { get; set; } = quizTitle;

        public long ActiveQuizId { get; set; } = Id; 
    }

}