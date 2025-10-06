using System.ComponentModel.DataAnnotations;

namespace Student_RegistrationPage.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name must contain only letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name must contain only letters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Roll number must contain only digits")]
        public string RollNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
         public string Gender { get; set; }
        public string ImagePath { get; set; } = "default.png";
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
