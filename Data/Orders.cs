namespace Cashbox.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            OrderProduct = new HashSet<OrderProduct>();
        }

        public int id { get; set; }

        public DateTime sell_datetime { get; set; }

        public int payment_method_id { get; set; }

        public int user_id { get; set; }

        public double sell_cost { get; set; }

        public double discount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProduct { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual Users Users { get; set; }
    }
}
