namespace Cashbox.MVVM.Models;

public partial class User
{
    public int Id { get; set; }

    public int Pin { get; set; }

    public virtual ICollection<AuthHistory> AuthHistories { get; set; } = [];

    public virtual ICollection<DailyReport> DailyReports { get; set; } = [];

    public virtual ICollection<Order> Orders { get; set; } = [];

    public virtual UserInfo? UserInfo { get; set; }
}
