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

    private async Task LoadQuizIdFromActiveQuiz(List<long?> activeQuizIds) {
        try {
            // Fetch QuizId and StartTime
            var quizIdAndTime = await MauiProgram.BusinessLogic.GetQuizIdsAndStartTimesByActiveQuizIds(activeQuizIds);

            // Check if data was found
            if (quizIdAndTime != null && quizIdAndTime.Any()) {
                // Sort the quizzes by StartTime from most recent to oldest
                var sortedQuizList = quizIdAndTime.OrderByDescending(q => q.StartTime).ToList();
                //Console.WriteLine("Fetched and Sorted Quiz IDs and Start Times (Most Recent First):");
                //foreach (var quiz in sortedQuizList) {
                //    Console.WriteLine($"QuizId: {quiz.QuizId}, StartTime: {quiz.StartTime}");
                //}

                Quizzes.Clear();

                foreach (var quiz in sortedQuizList) {
                    // Create a new History object for each quiz - storing QuizId and StartTime
                    Quizzes.Add(new History(quiz.QuizId, quiz.StartTime));
                }
            } else {
                await DisplayAlert("No Quizzes", "No quizzes found for the active quizzes.", "OK");
            }

        } catch (Exception ex) {
            await DisplayAlert("Error", $"Failed to load QuizId: {ex.Message}", "OK");
        }
    }



    // History class to store quiz information
    public class History {
        public DateTime StartTime { get; set; }
        public long? QuizId { get; set; }

        // Constructor for active quiz ID
        public History(long? quizId, DateTime startTime) {
            QuizId = quizId;
            StartTime = startTime;
        }

    }
}
