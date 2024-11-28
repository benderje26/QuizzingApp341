using QuizzingApp341.Views;

namespace QuizzingApp341;

public class UserInterfaceUtil {
    public static async Task ProcessResponseResult(bool success, Page page) {
        if (success) {
            await page.Navigation.PushAsync(new WaitScreen("Waiting for next question..."), false);
            page.Navigation.RemovePage(page);
        } else {
            await page.DisplayAlert("Error", "Something went wrong :(", "OK");
        }
    }
}