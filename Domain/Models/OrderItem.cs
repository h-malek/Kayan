using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("order_items", Schema = "sales")]
    public class OrderItem
    {
        [Column("order_item_id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("list_price")]
        public decimal UnitPrice { get; set; }
    }
}
