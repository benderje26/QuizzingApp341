namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Linq;
using System.Threading.Tasks;

public partial class QuizHistory : ContentPage {
    private readonly IBusinessLogic _businessLogic;
    public IBusinessLogic BusinessLogic => _businessLogic;

    public bool TakenSelected {
        get => takenSelected;
        set {
            if (takenSelected != value) {
                takenSelected = value;
                OnPropertyChanged(nameof(TakenSelected));
                OnPropertyChanged(nameof(ActivatedSelected));
            }
        }
    }
    private bool takenSelected;
    public bool ActivatedSelected {
        get => !TakenSelected;
        set => TakenSelected = !value;
    }

    public QuizHistory() {
        _businessLogic = MauiProgram.BusinessLogic;
        InitializeComponent();
        BindingContext = this;
    }

    private void OnTakenButtonClicked(object sender, EventArgs e) {
        TakenSelected = true;
    }

    private void OnActivatedButtonClicked(object sender, EventArgs e) {
        ActivatedSelected = true;
    }

    private async void OnStudyButtonClicked(object sender, EventArgs e) {
        if (sender is Button button) {
            ActiveQuiz? quiz = null;
            if (button.BindingContext is Models.Participant p && p.IsUserOwner) {
                quiz = p.ActiveQuiz;
            } else if (button.BindingContext is ActiveQuiz aq) {
                quiz = aq;
            }

            if (quiz != null) {
                // Navigate to study page
            }
        }
    }

    private async void OnViewStatsButtonClicked(object sender, EventArgs e) {
        if (sender is Button button) {
            ActiveQuiz? quiz = null;
            if (button.BindingContext is Models.Participant p && p.IsUserOwner) {
                quiz = p.ActiveQuiz;
            } else if (button.BindingContext is ActiveQuiz aq) {
                quiz = aq;
            }

            if (quiz != null) {
                // Create a new StatisticsScreen instance and push it on the stack
                var quizStats = await MauiProgram.BusinessLogic.GetQuizScoresForActiveQuizId(quiz.Id);
                await Navigation.PushAsync(new StatisticsScreen(quizStats));
            }
        }
    }


    private async void OnDeleteButtonClicked(object sender, EventArgs e) {
        if (sender is Button button) {
            ActiveQuiz? quiz = null;
            if (button.BindingContext is Models.Participant p) {
                await DisplayAlert("Sorry", "You cannot delete your participant record.", "OK");
            } else if (button.BindingContext is ActiveQuiz aq) {
                quiz = aq;
            }

            if (quiz != null) {
                // Double check they want to delete it
                bool shouldDelete = await DisplayAlert("Are you sure?", "Are you sure you want to delete this quiz record? This cannot be undone.", "YES", "NO");
                
                if (shouldDelete) {
                    // Attempt to delete it
                    bool quizDeleted = await MauiProgram.BusinessLogic.DeleteQuizFromActivationHistory(quiz.Id);
                    if (!quizDeleted) {
                        await DisplayAlert("Uh Oh!", "Could not delete the quiz record. Something went wrong.", "OK");
                    }
                }
            }
        }
    }

}
