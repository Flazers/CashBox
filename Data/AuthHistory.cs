namespace Cashbox.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AuthHistory")]
    public partial class AuthHistory
    {
        public int id { get; set; }

        public DateTime datetime { get; set; }

        public int user_id { get; set; }

        public virtual Users Users { get; set; }
    }
}
