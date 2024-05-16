using System.ComponentModel.DataAnnotations;

namespace Cashbox.MVVM.Models;

public partial class AppSettings
{
    [Key]
    public int Id { get; set; }

    public int Salary { get; set; }
    public int StartCash { get; set; }
}
