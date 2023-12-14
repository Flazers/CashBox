using System;
using System.Collections.Generic;

namespace Cashbox.MVVM.Models;

public partial class Tfadatum
{
    public int UserId { get; set; }

    public string Code { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
