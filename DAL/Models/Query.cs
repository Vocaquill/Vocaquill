using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Query
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(5000, ErrorMessage = "Request must be 5000 characters or less")]
        public string Request { get; set; }

        [Required]
        public DateTime RequestTime { get; set; }

        [Required]
        [MaxLength(10000, ErrorMessage = "Response must be 10000 characters or less")]
        public string Response { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
