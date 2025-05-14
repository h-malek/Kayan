using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("stores", Schema = "sales")]
    public class Store
    {
        [Column("store_id")]
        public int Id { get; set; }

        [Column("store_name")]
        public string StoreName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
