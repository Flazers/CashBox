namespace Cashbox.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AutoDReport")]
    public partial class AutoDReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int daily_report_id { get; set; }

        public double salary { get; set; }

        public double auto_proceeds { get; set; }

        public double forfeit { get; set; }

        public double award { get; set; }

        public virtual DailyReport DailyReport { get; set; }
    }
}
