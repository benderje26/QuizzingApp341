namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public partial class QuizStudio : INotifyPropertyChanged {
    public ICommand EditQuizCommand { get; set; }
    public ObservableCollection<Quiz> CreatedQuizzes { get; set; }
    Guid? userID { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public QuizStudio() {
        InitializeComponent();
        CreatedQuizzes = MauiProgram.BusinessLogic.UserInfo?.CreatedQuizzes ?? [];
        EditQuizCommand = new Command<Quiz>(EditQuiz);
        userID = MauiProgram.BusinessLogic.UserInfo?.Id;
        // Subscribe to the "QuizUpdated" message
        MessagingCenter.Subscribe<EditQuiz, Quiz>(this, "QuizUpdated", (sender, updatedQuiz) => {
            // Find the quiz in the CreatedQuizzes collection and update it
            CreatedQuizzes.FirstOrDefault(q => q.Id == updatedQuiz.Id).Title = updatedQuiz.Title;
            Console.WriteLine("**************************");
            Console.WriteLine("Quiz Title Changed to: " + CreatedQuizzes.FirstOrDefault(q => q.Id == updatedQuiz.Id).Title);
            // Refresh the UI if needed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CreatedQuizzes"));
            // OnPropertyChanged(nameof(CreatedQuizzes));
        });
        BindingContext = this;
    }

    // When this screen is visible listen to the quiz updated function to update the quiz
    protected override void OnAppearing() {
        base.OnAppearing();
        // Subscribe to the "QuizUpdated" message
        MessagingCenter.Subscribe<EditQuiz, Quiz>(this, "QuizUpdated", (sender, updatedQuiz) => {
            // Find the quiz in the CreatedQuizzes collection and update it
            CreatedQuizzes.FirstOrDefault(q => q.Id == updatedQuiz.Id).Title = updatedQuiz.Title;

            // Refresh the UI if needed
            OnPropertyChanged(nameof(CreatedQuizzes));
        });
    }

    // When the screen is not visible, stop listening to the quiz updated function
    protected override void OnDisappearing() {
        base.OnDisappearing();
        // Unsubscribe from the message
        MessagingCenter.Unsubscribe<EditQuiz, Quiz>(this, "QuizUpdated");
    }

    private async void EditQuiz(Quiz quiz) {
        QuizManager quizManager = new QuizManager(quiz);
        var test = await quizManager.GetQuestions();

        await Navigation.PushAsync(new EditQuiz(quizManager));
    }


    private void OnCreateNewQuizButtonClicked(object sender, EventArgs e) {
        Navigation.PushAsync(new CreateNewQuiz());
    }

}