using System;
using System.Collections.Generic;

namespace Cashbox.MVVM.Models;

public partial class AutoDreport
{
    public int DailyReportId { get; set; }

    public int Salary { get; set; }

    public double? AutoProceeds { get; set; }

    public double? Forfeit { get; set; }

    public double? Award { get; set; }

    public double? FullTransit { get; set; }

    public virtual DailyReport DailyReport { get; set; } = null!;
}
