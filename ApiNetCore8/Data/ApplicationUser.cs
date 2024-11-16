﻿using Microsoft.AspNetCore.Identity;

namespace ApiNetCore8.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Position { get; set; } = null!;
    }
}
