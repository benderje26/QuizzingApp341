using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[Table("quizzes")]
public class Quiz : BaseModel, INotifyPropertyChanged {
    private long id;
    private Guid creatorId;
    private string? title;

    [PrimaryKey("id")]
    public long Id {
        get => id;
        set {
            id = value;
            OnPropertyChanged();
        }
    }

    [Column("creator_id")]
    public Guid CreatorId { 
        get => creatorId;
        set {
            creatorId = value;
            OnPropertyChanged();
        }
     }

    [Column("title")]
    public string Title {
        get => title;
        set {
            title = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}