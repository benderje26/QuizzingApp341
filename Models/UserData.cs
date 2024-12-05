using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace QuizzingApp341.Models;

[Table("user_data")]
public class UserData : BaseModel {
    [PrimaryKey("user_id", true)]
    public Guid UserId { get; set; }

    [Column("username")]
    public string Username { get; set; } = string.Empty;
}
