using QuizzingApp341.Models;
using Microsoft.Maui.Controls;
using QuizzingApp341.Views;

namespace QuizzingApp341;

public class UserInterfaceUtil {
    public static async Task ProcessActiveQuizEnded(INavigation navigation) {
        // Leave the quiz (you can still get back in by re-entering the code)
        MauiProgram.BusinessLogic.LeaveActiveQuiz();

        await navigation.PopToRootAsync();
    }

    public static async Task ProcessResponseResult(bool success, Page page) {
        if (success) {
            await page.Navigation.PushAsync(new WaitScreen("Waiting for next question..."), false);
            page.Navigation.RemovePage(page);
        } else {
            await page.DisplayAlert("Error", "Something went wrong :(", "OK");
        }
    }
}