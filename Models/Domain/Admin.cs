using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercuryShopping.Models.Tables
{
    public class Admin
    {
        [Key][Column(TypeName = "nvarchar(20)")]
        public String? UserName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public String? PassWord { get; set; }
    }
}
