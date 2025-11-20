using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Mentor
{
    public class SearchMentorDto
    {
        public List<string>? Skills { get; set; }
        public string? Location { get; set; }
        public string? SearchTerm { get; set; } 
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
