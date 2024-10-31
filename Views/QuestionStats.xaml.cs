namespace QuizzingApp341.Views;
using Microsoft.Maui.Controls.PlatformConfiguration;
using QuizzingApp341.Models;
using System.Collections.Generic;

/*
 * Name: Peter Skogman
 */

public partial class QuestionStats : ContentPage
{
	public QuestionStats()
	{
		InitializeComponent();
	}

    private void OnNextQuestionClicked(object sender, EventArgs e)
    {
        // Navigate to Create account
        Navigation.PushAsync(new FillBlank());
    }

//    //private bool CheckIfMultipleChoice(string question) {
//    //    return true;
//    //}
//}
