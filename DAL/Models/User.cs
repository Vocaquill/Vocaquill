﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Login must be 30 characters or less"), MinLength(5, ErrorMessage = "Login must be 5 characters or more")]
        public string Login { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Login must be 50 characters or less"), MinLength(8, ErrorMessage = "Login must be 8 characters or more")]
        public string Password { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Login must be 30 characters or less")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool isDeleted { get; set; } = false;
        public ICollection<Query> Queries { get; set; }
    }
}
