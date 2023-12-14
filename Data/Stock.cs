using System;
using System.Collections.Generic;

namespace Cashbox.Data;

public partial class Stock
{
    public int ProductId { get; set; }

    public int Amount { get; set; }

    public virtual Product Product { get; set; } = null!;
}
