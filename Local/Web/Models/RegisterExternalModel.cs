using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class RegisterExternalModel
    {
        [Required(ErrorMessage = "Have to supply a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Have to supply an e-mail address")]
        public string Email { get; set; }
    }
}
