using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class Refund
{
    [Key]
    public int Id { get; set; }

    public int DailyReportId { get; set; }

    public int? ProductId { get; set; }

    public string? Reason { get; set; } = null!;

    public DateOnly? BuyDate { get; set; }

    public bool IsPurchased { get; set; }

    public bool IsSuccessRefund { get; set; }

    public virtual Product? Product { get; set; } = null!;
    public virtual DailyReport DailyReport { get; set; } = null!;
}
