namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
// This class initializes and displays the Quiz history.
public partial class QuizHistory : ContentPage {
    public List<Quiz> Quizzes { get; set; }

    public QuizHistory() {
        InitializeComponent();

        Quizzes = new List<Quiz>
        {
            new Quiz ("Quiz 1", DateTime.Parse("2024-01-01") ),
            new Quiz ("Quiz 2", DateTime.Parse("2024-01-02") ),
            new Quiz ("Quiz 3", DateTime.Parse("2024-01-03") ),
            new Quiz ("Quiz 4", DateTime.Parse("2024-01-04") )
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