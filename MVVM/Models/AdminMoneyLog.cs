namespace Cashbox.MVVM.Models;

public partial class AdminMoneyLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime Datetime { get; set; }

    public string Action { get; set; } = string.Empty;

    public double Money { get; set; }

    public int? SubUserId { get; set; }

    public virtual User? SubUser { get; set; } = null;
    public virtual User User { get; set; } = null!;
}
