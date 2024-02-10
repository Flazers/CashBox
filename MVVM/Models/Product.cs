using System;
using System.Collections.Generic;

namespace Cashbox.MVVM.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? ArticulCode { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public byte[]? Image { get; set; }

    public string Brand { get; set; } = null!;

    public int CategoryId { get; set; }

    public double PurchaseСost { get; set; }

    public double SellCost { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual Stock? Stock { get; set; }
}
