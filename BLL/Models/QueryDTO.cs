using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class QueryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Request { get; set; }
        public DateTime RequestTime { get; set; }
        public string Response { get; set; }
        public int UserId { get; set; }
    }
}
