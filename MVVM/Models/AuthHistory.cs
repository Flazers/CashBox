using System;
using System.Collections.Generic;

namespace Cashbox.MVVM.Models;

public partial class AuthHistory
{
    public int Id { get; set; }

    public DateTime Datetime { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
