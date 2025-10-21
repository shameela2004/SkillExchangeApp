using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Interfaces.Repositories;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        //public async Task UpdateAsync(int id, T updatedEntity)
        //{
        //    var entity = await _repository.GetByIdAsync(id);
        //    if (entity == null) throw new KeyNotFoundException($"Entity with id {id} not found");

        //    _repository.Update(updatedEntity);
        //    await _repository.SaveChangesAsync();
        //}
        public async Task UpdateAsync(int id, UpdateSkillRequest request)
        {
            // Step 1: Get the existing entity from the database
            var skill = await _repository.GetByIdAsync(id);
            if (skill == null)
                throw new KeyNotFoundException($"Skill with id {id} not found");

            // Step 2: Update its properties
            skill.Name = request.Name;
            skill.LastUpdatedAt = DateTime.UtcNow;

            // Step 3: Save changes (EF already tracks this entity)
            await _repository.SaveChangesAsync();
        }







        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _repository.Remove(entity);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
