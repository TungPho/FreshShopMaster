using System.ComponentModel.DataAnnotations;

namespace FreshShopMaster.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [StringLength(100)]
        public string CategoryName { get; set; }
        [StringLength(1000)]
        public string CategoryPhoto { get; set; }

        public int CategoryOrder { get; set; }
    }
}
