namespace FreshShopMaster.Models.ViewModels
{
    public class ProductListViewModel
    {

        public IEnumerable<Product> Products { get; set; }

        public PagingInfo PaingInfo  { get; set; } = new PagingInfo();
    }
}
