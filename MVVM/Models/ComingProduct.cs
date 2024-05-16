using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class ComingProduct
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CommingDatetime { get; set; }
    public double BuyCost { get; set; }
    public virtual User User { get; set; } = null!;
}
