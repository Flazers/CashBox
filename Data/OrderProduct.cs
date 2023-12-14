namespace Cashbox.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrderProduct")]
    public partial class OrderProduct
    {
        public int id { get; set; }

        public int order_id { get; set; }

        public int product_id { get; set; }

        public int amount { get; set; }

        public double purchase_сost { get; set; }

        public double sell_cost { get; set; }

        public virtual Orders Orders { get; set; }

        public virtual Product Product { get; set; }
    }
}
