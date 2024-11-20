namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
// This class initializes and displays the Quiz history.
public partial class QuizHistory : ContentPage {
    public List<QuizManager> Quizzes { get; set; }

    public QuizHistory() {
        InitializeComponent();

        // TODO Get the user's quiz history (should include all the quizzes they've taken and quizzes they've activated)
        Quizzes = new List<QuizManager>
        {
        };

        BindingContext = this;
    }

    async void OnQuizTapped(object sender, EventArgs e) {
        if (sender is StackLayout stackLayout &&
            stackLayout.GestureRecognizers[0] is TapGestureRecognizer tapGesture &&
            tapGesture.CommandParameter is QuizManager tappedQuiz) {
            // Navigate or perform any action with tappedQuiz
            await Navigation.PushAsync(new StatisticsScreen());
        }
    }
}