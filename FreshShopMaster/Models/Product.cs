using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FreshShopMaster.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [StringLength(100)]
        public string ProductName  { get; set; }
        
        [StringLength(3000)]
        [AllowNull]
		[Required(AllowEmptyStrings = true)]
        [ValidateNever]
		public string ProductPhoto { get; set; }

        public string ProductDescription { get; set; }
        [Column(TypeName ="decimal(8,2)")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "decimal(2,2)")]
        public decimal ProductDiscount { get; set; }
        [StringLength(100)]
        public string ProductType { get; set; }
        
        [ForeignKey("Order")]
		public int OrderId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }


        // Other cols not in Db
        [NotMapped]
       public IFormFile Photo { get; set; }

    }
}
