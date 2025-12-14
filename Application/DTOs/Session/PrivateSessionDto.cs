using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Session
{
    public class PrivateSesionDto : SessionDto
    {
        public string? VideoLink { get; set; }  // only for mentor + booked learner
    }
}
