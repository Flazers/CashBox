namespace Cashbox.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Refund")]
    public partial class Refund
    {
        public int id { get; set; }

        public int product_id { get; set; }

        [Required]
        [StringLength(50)]
        public string reason { get; set; }

        [Column(TypeName = "date")]
        public DateTime buy_date { get; set; }

        public bool isPurchased { get; set; }

        public virtual Product Product { get; set; }
    }
}
