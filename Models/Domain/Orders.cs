using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercuryShopping.Models.Tables
{
    public class Orders
    {
        
        [Required] [Key]
        public int OrderID { get; set; }

        [Required]
        public int CustID { get; set; }

        public double? Total { get; set; }

        [Required]
        public DateTime Date_ { get; set; }



        [Required] [Column(TypeName = "nvarchar(500)")]
        public String? Address { get; set; }

        [Required]
        public int? OrderStatus { get; set; }
        //-----------------------------------------------------------


        [JsonIgnore]
        public  List<OrderDetails>? OrderDetails { get; set; }
 
    }
}
