using System;
using System.Collections.Generic;

namespace Cashbox.Data;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
