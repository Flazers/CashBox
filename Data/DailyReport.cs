namespace Cashbox.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DailyReport")]
    public partial class DailyReport
    {
        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime data { get; set; }

        public TimeSpan open_time { get; set; }

        public TimeSpan close_time { get; set; }

        public int user_id { get; set; }

        public double proceeds { get; set; }

        public virtual AutoDReport AutoDReport { get; set; }

        public virtual Users Users { get; set; }
    }
}
