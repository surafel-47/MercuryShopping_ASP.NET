using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MercuryShopping.Models.Tables
{
    public class Reviews
    {
        [Key]
        public int ReviewID { get; set; }
        [Required]
        public int CustID { get; set; }

        [Required]
        public int ProID { get; set; }


        [Required]
        public int Rating { get; set; }

        [Column(TypeName = "nvarchar(500)")] [Required]
        public String? Comment { get; set; }

        [NotMapped]
        public String? ReviewerFullName { get; set; }


        //-------------------------------------------------------------------------
        [ForeignKey("CustID")] [Required][JsonIgnore]
        public Customer? Customer { get; set; }


        [ForeignKey("ProID")] [Required] [JsonIgnore]
        public Product? Product { get; set; }
    }
}
