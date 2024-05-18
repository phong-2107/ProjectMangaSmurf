namespace ProjectMangaSmurf.Models.ViewModels
{
    public class StaffDetailsViewModel
    {
        public User User { get; set; }
        public NhanVien Staff { get; set; }
        public string PlainPassword { get; set; }
        public bool IsPasswordValid { get; set; }
        public List<StaffPermissionsDetail> Permissions { get; set; } 
    }
}

