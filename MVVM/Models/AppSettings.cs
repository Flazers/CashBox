using System;
using System.Collections.Generic;

namespace Cashbox.MVVM.Models;

public partial class AppSettings
{
    public int Id { get; set; }

    public double? MoneyBox { get; set; }

    public string? MainEmail { get; set; } = string.Empty;

    public int? Salary { get; set; }

    public double? AwardProcent { get; set; }
}
