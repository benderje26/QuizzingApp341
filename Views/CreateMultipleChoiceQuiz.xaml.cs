namespace QuizzingApp341.Views;
using System;
using Microsoft.Maui.Controls;
using QuizzingApp341.Models;

public partial class CreateMultipleChoiceQuiz : ContentPage {
    public CreateMultipleChoiceQuiz() {
        InitializeComponent();
    }

    private void OnSaveClicked(object sender, EventArgs e) {
        // Retrieve data from user input
        //check for null, if null - replace with empty string
        string question = questionMultipleChoice.Text != null ? questionMultipleChoice.Text.Trim() : string.Empty;
        string optionA = optionAEntry.Text != null ? optionAEntry.Text.Trim() : string.Empty;
        string optionB = optionBEntry.Text != null ? optionBEntry.Text.Trim() : string.Empty;
        string optionC = optionCEntry.Text != null ? optionCEntry.Text.Trim() : string.Empty;
        string optionD = optionDEntry.Text != null ? optionDEntry.Text.Trim() : string.Empty;
        string correctAnswer = correctAnswerFromUser.SelectedItem != null ? correctAnswerFromUser.SelectedItem.ToString().Trim() : string.Empty;


        // Check for required fields not empty
        if (string.IsNullOrEmpty(question)) {
            DisplayAlert("Error", "Please enter a question.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(optionA) || string.IsNullOrEmpty(optionB) ||
            string.IsNullOrEmpty(optionC) || string.IsNullOrEmpty(optionD)) {
            DisplayAlert("Error", "Please provide all options.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(correctAnswer)) {
            DisplayAlert("Error", "Please select the correct answer.", "OK");
            return;
        }

        // Do something with the retrieved data - make a question object - saving to Database

        // Navigate back to the CreateNewQuiz page
        var newPage = new CreateNewQuiz();
        Navigation.InsertPageBefore(newPage, this); 
        Navigation.PopAsync();  
    }


   
}
