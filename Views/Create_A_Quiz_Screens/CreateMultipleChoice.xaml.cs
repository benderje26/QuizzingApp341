namespace QuizzingApp341.Views;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls;
using QuizzingApp341.Models;

public partial class CreateMultipleChoice : ContentPage {
    public string ScreenTitle => IsNewQuestion ? "Create Question" : "Edit Question";
    public Question Question { get; set; }
    public ObservableCollection<IndexValuePair> Options { get; set; }
    private IndexValuePair? lastSelected;
    public bool Multiselect {
        get => multiselect;
        set {
            multiselect = value;
            if (!value && Options.Where(x => x.IsSelected).Count() > 1) {
                foreach (var option in Options) {
                    option.IsSelected = false;
                }
            }
            OnPropertyChanged(nameof(Multiselect));
            OnPropertyChanged(nameof(NotMultiselect));
        }
    }
    private bool multiselect;
    public bool NotMultiselect => !Multiselect;

    public string QuestionText { get; set; } = string.Empty;

    public bool IsNewQuestion { get; set; }

    public bool IsEditQuestion => !IsNewQuestion;
    public CreateMultipleChoice(Question? question, bool isNewQuestion) {
        IsNewQuestion = isNewQuestion;

        // If there is a question present to edit
        if (question != null) {
            Question = question;
            QuestionText = question.QuestionText ?? string.Empty;

            // Set all the options with the question's current answer options
            var selected = question.MultipleChoiceCorrectAnswers ?? [];
            Options = new ObservableCollection<IndexValuePair>
                ((question.MultipleChoiceOptions ?? [])
                    .Select((val, index) => new IndexValuePair(index, val) {
                        IsSelected = selected.Contains(index)
                    }));
            Multiselect = question.Multiselect ?? false;
        } else {
            Question = new() { QuestionType = QuestionType.MultipleChoice };
            Options = [];
        }

        InitializeComponent();
        BindingContext = this;
    }

    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.BindingContext is IndexValuePair value) {
            value.IsSelected = e.Value;
            lastSelected = value;
        }
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (sender is RadioButton radio && radio.BindingContext is IndexValuePair value) {
            if (Multiselect) {
                return;
            }
            value.IsSelected = e.Value;
            lastSelected = value;
        }
    }

    private void OnRemoveOptionClicked(object sender, EventArgs e) {
        BindableObject? obj = sender as BindableObject;

        if (obj?.BindingContext is IndexValuePair value) {
            int index = value.Index;
            Options.RemoveAt(index);
            foreach (var option in Options) {
                if (option.Index > index) {
                    option.Index--;
                }
            }
        }
    }

    private void OnAddOptionClicked(object sender, EventArgs e) {
        Options.Add(new IndexValuePair(Options.Count, string.Empty));
    }

    private bool RetrieveData() {
        // Retrieve data from user input
        string question = questionName.Text.Trim();
        string[] options = Options
            .Select(x => x.Value.Trim())
            .ToArray();
        int[] selected = Options
            .Where(x => x.IsSelected)
            .Select(x => x.Index)
            .ToArray();

        // Check for required fields not empty
        if (string.IsNullOrEmpty(question)) {
            DisplayAlert("Error", "Please enter a question.", "OK");
            return false;
        }

        if (Options.Count == 0) {
            DisplayAlert("Error", "Please make an option.", "OK");
            return false;
        }

        if (selected.Length == 0) {
            DisplayAlert("Error", "Please select a correct answer.", "OK");
            return false;
        }

        Question.QuestionText = question;
        Question.MultipleChoiceOptions = options;
        Question.MultipleChoiceCorrectAnswers = selected;
        Question.Multiselect = Multiselect;
        return true;
    }
    private async void OnSaveClicked(object sender, EventArgs e) {
        bool goodToSave = RetrieveData();
        if (!goodToSave) {
            return;
        }

        if (IsNewQuestion) {
            await MauiProgram.BusinessLogic.AddQuestion(Question);
        } else {
            await MauiProgram.BusinessLogic.EditQuestion(Question);
        }

        // Navigate back to the CreateNewQuiz page
        await Navigation.PopAsync();
    }

    private async void OnDeleteQuestionClicked(object sender, EventArgs e) {
        bool deleteQuestion = await DisplayAlert("Are you sure you would like to delete this question?", Question.QuestionText, "Yes", "No");
        if (deleteQuestion) {
            var result = await MauiProgram.BusinessLogic.DeleteQuestion(Question.Id);
            // Navigate back to the CreateNewQuiz page
            if (result.Item1 != DeleteQuestionResult.Success) {
                await DisplayAlert("Error. Cannot delete question", result.Item2, "Ok");
            } else {
                // Navigate back to the CreateNewQuiz page
                await Navigation.PopAsync();
            }
        }
    }
}
