﻿namespace EmployeeManagementSystem.Domain.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}