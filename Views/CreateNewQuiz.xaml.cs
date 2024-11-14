using CommunityToolkit.Maui.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;

namespace QuizzingApp341.Views {
    public partial class CreateNewQuiz : ContentPage {
        public ObservableCollection<Object> Questions { get; set; } = new ObservableCollection<Object>();
        public CreateNewQuiz() {
            InitializeComponent();

            // Subscribe to the message and add the question to the collection
            MessagingCenter.Subscribe<CreateMultipleChoiceQuiz, MultipleChoiceQuestion>(this, "AddQuestion", (sender, question) => {
                Questions.Add(question);
                Console.WriteLine($"Question added: {question.Text}");
            });

            BindingContext = this;
        }

        private async void OnQuestionTypeButtonClicked(object sender, EventArgs e) {
            var popup = new CreateNewQuizPopup();

            popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
                if (questionType == "MultipleChoice") {

                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateMultipleChoiceQuiz());

                } else if (questionType == "FillInBlank") {

                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateFillBlank());
                }
            };

            // Show the popup
            await this.ShowPopupAsync(popup);
        }

        private async void OnSaveNewQuizClicked(object sender, EventArgs e) {
            var newPage = new MyQuiz();

            //completely reset the navigation stack
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new NavigationPage(newPage));
        }
    }
}