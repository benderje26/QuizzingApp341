using Microsoft.Maui.Controls.PlatformConfiguration;

namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class MultipleChoice : ContentPage
{
	public MultipleChoice()
	{
		InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }
}