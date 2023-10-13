using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercuryShopping.Models.Tables
{
    public class Catagory
    {
        [Key]
        public int CatagID { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public String? CatagName { get; set; }

        //-------------------------------------------------------------
        public virtual List<Product>? Products { get; set; }
    }
}
