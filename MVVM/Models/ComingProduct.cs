namespace Cashbox.MVVM.Models;

public partial class ComingProduct
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CommingDatetime { get; set; }
    public double BuyCost { get; set; }
    public virtual User User { get; set; } = null!;
}
