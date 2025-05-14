using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("products", Schema = "production")]
    public class Product
    {
        [Column("product_id")]
        public int Id { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("brand_id")]
        public int BrandId { get; set; }
        //public virtual Brand Brand { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
