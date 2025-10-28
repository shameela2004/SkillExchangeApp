using MyApp1.Application.DTOs.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface ISkillService
    {
        Task UpdateSkillAsync(int id, UpdateSkillRequest dto);
    }
}
