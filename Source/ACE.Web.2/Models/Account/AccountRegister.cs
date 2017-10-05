using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACE.Web.Models.Account
{
    /// <summary>
    /// view model for registering a new account
    /// </summary>
    public class AccountRegister
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {4} characters long.", MinimumLength = 6)]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirmation { get; set; }
    }
}