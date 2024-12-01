using System;

namespace AuthAPI.Models
{
    public class Graduate
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // GUID for the Graduate Id
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDeleted { get; set; } = false; // Flag for soft delete
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
