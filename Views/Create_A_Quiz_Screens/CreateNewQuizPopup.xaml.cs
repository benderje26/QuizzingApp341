using CommunityToolkit.Maui.Views;

namespace QuizzingApp341.Views {
    public partial class CreateNewQuizPopup : Popup {
        public event Action<string>? QuestionTypeSelected;

        public CreateNewQuizPopup() {
            InitializeComponent();
        }

        private void OnMultipleChoiceClicked(object sender, EventArgs e) {
            QuestionTypeSelected?.Invoke("MultipleChoice");
            Close(); 
        }

        private void OnFillInBlankClicked(object sender, EventArgs e) {
            QuestionTypeSelected?.Invoke("FillInBlank");
            Close(); 
        }

        private void Cancel_Clicked(object sender, EventArgs e) {
            Close();
        }
    }
}
