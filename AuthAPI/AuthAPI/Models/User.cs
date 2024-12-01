using Microsoft.AspNetCore.Identity;
using System;

namespace AuthAPI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public DateTime UserCreated { get; set; } = DateTime.UtcNow; // Automatically set to current time
        public DateTime? UserEdited { get; set; } // Nullable to track updates
    }
}
