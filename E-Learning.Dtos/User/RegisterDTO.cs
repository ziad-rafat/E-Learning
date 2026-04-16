using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace E_Learning.Dtos.User
{
    [NotMapped]
    public class RegisterDTO
    {
       // [Required]
        //public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Only Gmail addresses are allowed.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{5,}$",
        ErrorMessage = "Password must be at least 5 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string password { get; set; }
        //public IFormFile? userImage { get; set; }
    }
}
