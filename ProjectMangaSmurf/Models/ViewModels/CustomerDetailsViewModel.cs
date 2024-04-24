namespace ProjectMangaSmurf.Models.ViewModels
{
   

    public class CustomerDetailsViewModel
    {
        public KhachHang Customer { get; set; }
        public IEnumerable<HopDong> Orders { get; set; }
    }

}
