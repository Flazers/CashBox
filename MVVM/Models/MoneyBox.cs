using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class MoneyBox
{
    [Key]
    public int Id { get; set; }

    public int Money { get; set; }
}
