namespace AuthAPI.Models
{
    public class RegisterVM
    {
        public string FirstName { get; set; } // Changed from Name
        public string LastName { get; set; }  // Changed from Surname
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Age { get; set; } // Replaced DateOfBirth
    }

    public class ForgotPasswordVM
    {
        public string Email { get; set; }
    }

    public class ResetPasswordVM
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
