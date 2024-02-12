using System.Collections;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreContext _context;
    //to store all repositories in a hash table
    private Hashtable _repositories;
    public UnitOfWork(StoreContext context)
    {
        _context = context;
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {   
        //check if the repository is already in the hash table
        if(_repositories == null) _repositories = new Hashtable();
        var type = typeof(TEntity).Name;
        if(!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
            _repositories.Add(type, repositoryInstance);
        }
        return (IGenericRepository<TEntity>)_repositories[type];
    }
}
