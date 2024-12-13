using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[Table("active_quizzes")]
public class ActiveQuiz : BaseModel, INotifyPropertyChanged {
    [PrimaryKey("id")]
    public long Id { get; set; }

    [Column("quiz_id")]
    public long QuizId { get; set; }

    [Column("quiz_title")]
    public string? QuizTitle { get; set; }

    [Column("access_code")]
    public string? AccessCode { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    private int? currentQuestionNo;

    [Column("current_question_no")]
    public int? CurrentQuestionNo {
        get => currentQuestionNo;
        set {
            currentQuestionNo = value;
            OnPropertyChanged();
        }
    }

    [Column("activator")]
    public Guid Activator { get; set; }

    [Column("start_time")]
    public DateTime? StartTime { get; set; }

    [Column("end_time")]
    public DateTime? EndTime { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}