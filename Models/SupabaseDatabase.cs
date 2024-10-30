namespace QuizzingApp341.Models;
using Supabase;

class SupabaseDatabase : IDatabase {

    private const string REST_URL = "https://tcogwlqjinvzckjmnjhp.supabase.co";
    private const string API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRjb2d3bHFqa"
        + "W52emNram1uamhwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjkzNjc4NTIsImV4cCI6MjA0NDk0Mzg1Mn0.bzYJIRYJ3rtvEh3usrydy7M3"
        + "ES1J6C5iMgPwzlqnTp8";

    private Client Client { get; set; }

    public SupabaseDatabase() {
        Client = new(REST_URL, API_KEY, new Supabase.SupabaseOptions {
            AutoConnectRealtime = true // TODO Jeremiah maybe should be false?
        });
    }

    public async void Initialize() {
        // initialize Supabase
        await Client.InitializeAsync();
    }

    public List<Question> LoadQuestions() {
        return [
            new MultipleChoiceQuestion(0, "How many CS students does it take to screw in a lightbulb?", ["1", "3", "10", "30"], 3),
            new FillBlankQuestion(1, "What is our professor's name?", ["Dr. Rogers", "Professor Rogers"], false)];
    }
}
