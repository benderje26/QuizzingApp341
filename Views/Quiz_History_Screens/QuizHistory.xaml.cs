namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
// This class initializes and displays the Quiz history.
public partial class QuizHistory : ContentPage {
    public List<Quiz> Quizzes { get; set; }

    public QuizHistory() {
        InitializeComponent();

        Quizzes = new List<Quiz>
        {
        };

        BindingContext = this;
    }

    async void OnQuizTapped(object sender, EventArgs e) {
        if (sender is StackLayout stackLayout &&
            stackLayout.GestureRecognizers[0] is TapGestureRecognizer tapGesture &&
            tapGesture.CommandParameter is Quiz tappedQuiz) {
            // Navigate or perform any action with tappedQuiz
            await Navigation.PushAsync(new StatisticsScreen());
        }
    }
}