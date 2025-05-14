using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("orders", Schema = "sales")]
    public class Order
    {
        [Column("order_id")]
        public int Id { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Column("store_id")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
