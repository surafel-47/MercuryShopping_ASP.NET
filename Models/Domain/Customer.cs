using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MercuryShopping.Models.Tables
{
    public class Customer
    {
        [Key][Required]
        public int CustID { get; set; }

        [Required] [Column(TypeName = "nvarchar(20)")]
        public String? Fname { get; set; }

        [Required] [Column(TypeName = "nvarchar(20)")]
        public String? Lname { get; set; }



        [Required] [Column(TypeName = "nvarchar(100)")]
        public String? PassWord { get; set; }

        [Required] [Column(TypeName = "nvarchar(20)")]
        public String? Email { get; set; }
       
        [Required] [Column(TypeName = "nvarchar(20)")]
        public String? PhoneNumber { get; set; }
        public int CustStatus { get; set; }

        [Column(TypeName = "nvarchar(30)")] 
        public String? CustImg { get; set; }

        [JsonIgnore]
        public List<Reviews>? Reviews { get; set; }
    }
}
