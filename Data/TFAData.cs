namespace Cashbox.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TFAData")]
    public partial class TFAData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string code { get; set; }

        public virtual Users Users { get; set; }
    }
}
