﻿namespace Cashbox.MVVM.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime? SellDatetime { get; set; }

    public int? PaymentMethodId { get; set; }

    public int UserId { get; set; }

    public int DailyReportId { get; set; }

    public double SellCost { get; set; }

    public double Discount { get; set; }

    public virtual ICollection<OrderProduct>? OrderProducts { get; set; } = [];

    public virtual DailyReport DailyReport { get; set; } = null!;

    public virtual PaymentMethod? PaymentMethod { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
