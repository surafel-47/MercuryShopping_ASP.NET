using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MercuryShopping.Models.Tables
{
    public class Product
    {
        [Key] [Required]
        public int ProID { get; set; }

        [Required]
        public int CatagID { get; set; }

        [Column(TypeName = "nvarchar(30)")] [Required]
        public string? ProName { get; set; }

        [Required]
        public double UPrice { get; set; }

        [Required] 
        public int Qty { get; set; }

        [Column(TypeName = "nvarchar(500)")] [Required]
        public string? ProDesc { get; set; }

        public int? ProStatus { get; set; }

        public double? AvgRating { get; set; }

        public int? NumOfReviews { get; set; }

        public string? Img1 { get; set; }
        public string? Img2 { get; set; }
        public string? Img3 { get; set; }
   


        //----------------------------------------------------------------------
        [ForeignKey("CatagID")] [Required] [JsonIgnore]
        public Catagory? Catagory { get; set; }

        [JsonIgnore]
        public List<Reviews>? Reviews { get; set; }
        [JsonIgnore]
        public List<OrderDetails>? OrderDetails { get; set; }
    }

}
