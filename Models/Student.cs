using System.ComponentModel.DataAnnotations;

namespace Student_RegistrationPage.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        [Required]
        public string RollNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
         public string Gender { get; set; }
        public string ImagePath { get; set; } = "default.png";

    }
}
