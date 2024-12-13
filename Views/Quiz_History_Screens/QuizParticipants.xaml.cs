using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading;

namespace QuizzingApp341.Views;
// This screen initializes and displays the list of participants of a quiz
public partial class QuizParticipants : ContentPage {
    public List<Participant> Participants { get; set; }

    private readonly IFileSaver filSaver;

    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


    public QuizParticipants(Dictionary<string, int> quizStats, IFileSaver fileSaver) {
        InitializeComponent();
        Participants = new List<Participant>();
        foreach (KeyValuePair<string, int> user in quizStats) {
            Participants.Add(new Participant(user.Key, user.Value));
        }
        BindingContext = this;
        filSaver = fileSaver;
    }



    private async void OnDownloadFileClicked(object sender, EventArgs e) {
        try {
            if (Participants == null || !Participants.Any()) {
                await DisplayAlert("No Data", "There are no participants to download.", "OK");
                return;
            }

            // Ask user for the file format
            //public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
            string fileType = await DisplayActionSheet("Choose File Format", "Cancel", null, ".txt", ".csv");
            Console.WriteLine($"User selected file type: {fileType}");

            // Cancel if the user doesn't select anything
            if (fileType == "Cancel" || string.IsNullOrEmpty(fileType)) {
                return;
            }

            Console.WriteLine("Building file content...");

            // Create file content
            var fileContent = new StringBuilder();
            fileContent.AppendLine("User,Score");

            foreach (var participant in Participants) {
                Console.WriteLine($"User: {participant.User}, Score: {participant.Score}");
                fileContent.AppendLine($"{participant.User},{participant.Score}");
            }

       
            Console.WriteLine("Generated file content:");
            Console.WriteLine(fileContent.ToString());


            // Determine file name 
            string fileName = fileType == ".txt" ? "Participants.txt" : "Participants.csv";
            Console.WriteLine($"File name determined: {fileName}");

            // Convert the content to a MemoryStream
            //using var stream = new MemoryStream(Encoding.Default.(fileContent.ToString()));
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            // Get the directory where app can store data files. 
            // Ex:C:\Users\User\AppData\Local\MyApp\Participants.txt
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            Console.WriteLine($"Saving file to path: {filePath}");

            // Save the file using the IFileSaver
            var result = await filSaver.SaveAsync(fileName, stream, cancellationTokenSource.Token);

            if (result.IsSuccessful) {
                await DisplayAlert("Success", $"File saved to: {result.FilePath}", "OK");
                Console.WriteLine($"File successfully saved to: {result.FilePath}");
            } else {
                await DisplayAlert("Error", "File saving failed.", "OK");
                Console.WriteLine("Error saving file.");
            }
        } catch (OperationCanceledException) {
            await DisplayAlert("Canceled", "File saving operation was canceled.", "OK");
        } catch (Exception ex) {
            await DisplayAlert("Error", $"An error occurred while saving the file: {ex.Message}", "OK");
        }
    }


}



// Represents a participant of a quiz with a User name and a score
public class Participant {
    public string User { get; set; }
    public int Score { get; set; }
    public Participant(string user, int score) {
        this.User = user;
        this.Score = score;
    }
}