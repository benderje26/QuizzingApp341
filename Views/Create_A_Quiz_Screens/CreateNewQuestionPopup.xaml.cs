using CommunityToolkit.Maui.Views;
using QuizzingApp341.Models;

namespace QuizzingApp341.Views {
    public partial class CreateNewQuestionPopup : Popup {
        public event Action<QuestionType>? QuestionTypeSelected;

        public CreateNewQuestionPopup() {
            InitializeComponent();
        }

        private void OnMultipleChoiceClicked(object sender, EventArgs e) {
            QuestionTypeSelected?.Invoke(QuestionType.MultipleChoice);
            Close();
        }

        private void OnFillInBlankClicked(object sender, EventArgs e) {
            QuestionTypeSelected?.Invoke(QuestionType.FillBlank);
            Close();
        }

        private void Cancel_Clicked(object sender, EventArgs e) {
            Close();
        }
    }
}
