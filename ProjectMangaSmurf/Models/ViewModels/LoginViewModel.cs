using System.ComponentModel.DataAnnotations;

namespace ProjectMangaSmurf.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }  // Make sure this matches what's used in the view.

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }



}
