using MyApp1.Application.Exceptions;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;

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
    public async Task UpdateAsync(T entity)
    {
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new NotFoundException($"{typeof(T).Name} with id {id} not found.");
        }

        _repository.Remove(entity);
        await _repository.SaveChangesAsync();
    }
}
