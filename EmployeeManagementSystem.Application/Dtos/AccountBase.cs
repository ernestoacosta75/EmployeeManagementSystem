﻿using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Application.Dtos
{
    public class AccountBase
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }
    }
}