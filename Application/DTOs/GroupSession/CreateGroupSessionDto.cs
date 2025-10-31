﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.GroupSession
{
    public class CreateGroupSessionDto
    {
        public int GroupId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Mode { get; set; } = string.Empty; // Online / Offline
        public string? Notes { get; set; }
    }
}
