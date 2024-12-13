using QuizzingApp341.Models;
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

    public static async Task ProcessNextResult(ActiveQuestion? question, Page page, bool isUserActivator, bool isUserParticipant) {
        if (question != null) {
            if (question.QuestionType == QuestionType.MultipleChoice) {
                await page.Navigation.PushAsync(new MultipleChoice(question, isUserActivator, isUserParticipant));
            } else {
                await page.Navigation.PushAsync(new FillBlank(question, isUserActivator, isUserParticipant));
            }
            page.Navigation.RemovePage(page);
        } else {
            await page.DisplayAlert("Error", "Something went wrong :(", "OK");
        }
    }

    public static async Task ShowQuizResults(long activeQuizId, Page page, bool shouldRemoveCurrentPage = false) {
        // Create a new StatisticsScreen instance and push it on the stack
        var quizStats = await MauiProgram.BusinessLogic.GetQuizScoresForActiveQuizId(activeQuizId);
        var scores = quizStats.Item1;
        var totalQuestions = quizStats.Item2;
        if (scores != null && scores.Count > 0) {
            await page.Navigation.PushAsync(new StatisticsScreen(scores, totalQuestions));
        } else {
            _ = page.DisplayAlert("Oh No", "No responses were found for the quiz.", "OK");
        }
        if (shouldRemoveCurrentPage) {
            page.Navigation.RemovePage(page);
        }
    }

    public static async Task StudyQuiz(long quizId, INavigation navigation) {
        QuizManager manager = new(await MauiProgram.BusinessLogic.GetQuiz(quizId));

        if (manager.Quiz == null) {
            return;
        }

        await manager.GetQuestions();
        MauiProgram.BusinessLogic.QuizManager = manager;

        // Prepare the quiz
        await MauiProgram.BusinessLogic.PrepareActiveQuiz(false);

        await MauiProgram.BusinessLogic.ActivateActiveQuiz();

        if (manager.CurrentQuestion == null || manager.ActiveQuiz == null) {
            await MauiProgram.BusinessLogic.DeactivateQuiz();
            return;
        }

        // Display the page to start the quiz
        // Make the current question an active question
        ActiveQuestion activeQuestion = new(manager.CurrentQuestion, manager.ActiveQuiz.Id);
        if (manager.CurrentQuestion.QuestionType == QuestionType.MultipleChoice) {
            await navigation.PushAsync(new MultipleChoice(activeQuestion, true, true));
        } else {
            await navigation.PushAsync(new FillBlank(activeQuestion, true, true));
        }
    }
}