using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class PaymentMethod
{
    [Key]
    public int Id { get; set; }

    public string Method { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = [];
}
