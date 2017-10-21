using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACE.Web.Models.Account
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Username { get; set; }

        [Required]
        [StringLength(280)]
        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [PasswordPropertyText]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }
    }
}