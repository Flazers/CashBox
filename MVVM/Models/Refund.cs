using System;
using System.Collections.Generic;

namespace Cashbox.MVVM.Models;

public partial class Refund
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Reason { get; set; } = null!;

    public DateOnly? BuyDate { get; set; }

    public bool IsPurchased { get; set; }

    public bool IsSuccessRefund { get; set; }

    public virtual Product? Product { get; set; } = null!;
}
