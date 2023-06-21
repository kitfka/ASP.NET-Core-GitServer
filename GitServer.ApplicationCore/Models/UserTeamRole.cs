namespace GitServer.ApplicationCore.Models;

public class UserTeamRole
{
    public long UserID { get; set; }
    public long TeamID { get; set; }
    public bool IsAdministrator { get; set; }
    public virtual Team Team { get; set; }
    public virtual User User { get; set; }
}
