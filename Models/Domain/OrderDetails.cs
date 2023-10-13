using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MercuryShopping.Models.Tables
{
    public class OrderDetails
    {
        [Required]
        public int ProID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int Qty { get; set; }
        
        [Required]
        public double ProTotal { get; set; }

        //---------------------------------------------

        [ForeignKey("OrderID")] [JsonIgnore]
        public Orders? Orders { get; set; }

        [ForeignKey("ProID")] [JsonIgnore]
        public Product? Product { get; set; }


        // Derived property
        [NotMapped]
        public string? ProName { get; set; } 
    }
}
