using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class DailyReport
{
    [Key]
    public int Id { get; set; }

    public DateOnly? Data { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    public int UserId { get; set; }

    public int? Proceeds { get; set; }
    public bool TakedSalary { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = [];

    public virtual ICollection<Refund> Refunds { get; set; } = [];

    public virtual AutoDreport? AutoDreport { get; set; }

    public virtual User User { get; set; } = null!;
}
