using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public int CategoryId { get; set; }

    public double SellCost { get; set; }

    public int CountSell { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ProductCategory Category { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = [];

    public virtual ICollection<Refund> Refunds { get; set; } = [];

    public virtual Stock? Stock { get; set; }
}
