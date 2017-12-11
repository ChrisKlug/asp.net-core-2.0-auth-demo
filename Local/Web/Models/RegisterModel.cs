using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Have to supply a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Have to supply an e-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Have to supply a password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The repeat password did not seem correct")]
        public string RepeatPassword { get; set; }
    }
}
