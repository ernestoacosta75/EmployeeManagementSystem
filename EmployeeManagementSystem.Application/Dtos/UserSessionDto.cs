﻿namespace EmployeeManagementSystem.Application.Dtos
{
    public class UserSessionDto
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
