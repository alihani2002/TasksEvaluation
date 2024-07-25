using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Infrastructure.Data;

namespace TasksEvaluation.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();


        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Create(T model)
        {
            await DbSet.AddAsync(model);
            await SaveChangesAsync();
            return model;
        }

        public async Task Delete(T model)
        {
            DbSet.Remove(model);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll() => await DbSet.AsNoTracking().ToListAsync();

        public async Task<T> GetById<IdType>(IdType id)
        {
            var data = await DbSet.FindAsync(id);
            return data is null ? throw new InvalidOperationException("No data Found") : data;
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task Update(T model)
        {
            DbSet.Update(model);
            await SaveChangesAsync();
        }

        public async Task<Student> GetByEmail(string email)
        {
            var data = await _dbContext.Students.FirstOrDefaultAsync(t => t.Email == email);
            return data is null ? throw new InvalidOperationException("No data Found") : data;
        }

        public async Task<IEnumerable<T>> GetAllWhere(Expression<Func<T, bool>> match) => await DbSet.Where(match).AsNoTracking().ToListAsync();

    }
}

