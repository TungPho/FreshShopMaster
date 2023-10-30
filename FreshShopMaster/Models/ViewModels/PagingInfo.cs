namespace FreshShopMaster.Models.ViewModels
{
    public class PagingInfo
    {
        
        public int TotalItem { get; set; }
        public int ItemsPerPage { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages => (int)(TotalItem / ItemsPerPage);
    }
}
