using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMangaSmurf.Models.ViewModels
{
    public class StaffRegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public string FullName { get; set; }

        [Required]
        public DateOnly? Birth { get; set; }

        [Required]
        public byte Gender { get; set; }

        [Required]
        public bool StaffRole { get; set; }

        public IEnumerable<PermissionsList> AvailablePermissions { get; set; }

        public Dictionary<byte, bool> SelectedPermissions { get; set; } = new Dictionary<byte, bool>();
    }
}

