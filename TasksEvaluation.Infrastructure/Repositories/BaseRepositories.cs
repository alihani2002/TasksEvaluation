﻿using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.consts;
using TasksEvaluation.Infrastructure.Data;
using TasksEvaluation.Core.IRepositories;

namespace TasksEvaluation.Infrastructure.Repositories
{
    public class BaseRepositories<T> : IBaseRepositories<T> where T : class
    {
        protected readonly ApplicationDbContext _context;


        public BaseRepositories(ApplicationDbContext context)
        {
            _context = context;
        }


        public IQueryable<T> FilterFindAll(Expression<Func<T, bool>> predicate,
                                     Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                     Expression<Func<T, object>>? orderBy = null,
                                     string? orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            query = query.Where(predicate);

            if (orderBy != null)
            {
                query = orderByDirection == OrderBy.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            return query;
        }

        public IQueryable<T> FilterGetAll(bool withNoTracking = true, string? orderByDirection = OrderBy.Ascending, Expression<Func<T, object>>? orderBy = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (withNoTracking)
                query = query.AsNoTracking();


            if (orderBy != null)
            {
                query = orderByDirection == OrderBy.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            return query;
        }

        public async Task<IEnumerable<T>> GetAll(bool withNoTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();

            if (withNoTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public async Task<T?> GetById(int id) => await _context.Set<T>().FindAsync(id);
        public async Task<T?> GetByName(string name) => await _context.Set<T>().FindAsync(name);


        public async Task<T?> Find(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().SingleOrDefaultAsync(predicate);

        public async Task<T?> Find(Expression<Func<T, bool>> predicate, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<T?> Find(Expression<Func<T, bool>> predicate,
                Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            if (include is not null)
                query = include(query);

            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);

            if (orderBy is not null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);

            if (orderBy is not null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, object>>? orderBy = null, string? orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            if (include is not null)
                query = include(query);

            query = query.Where(predicate);

            if (orderBy is not null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            await _context.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
            return entities;
        }

        public void Update(T entity) => _context.Update(entity);

        //.NET 6
        public void Remove(T entity) => _context.Remove(entity);

        //.NET 6
        public void RemoveRange(IEnumerable<T> entities) => _context.RemoveRange(entities);

        //.NET 7
        public void DeleteBulk(Expression<Func<T, bool>> predicate) =>
            _context.Set<T>().Where(predicate).ExecuteDelete();

        public bool IsExists(Expression<Func<T, bool>> predicate) =>
            _context.Set<T>().Any(predicate);

        public int Count() => _context.Set<T>().Count();

        public int Count(Expression<Func<T, bool>> predicate) => _context.Set<T>().Count(predicate);

        public int Max(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field) =>
            _context.Set<T>().Any(predicate) ? _context.Set<T>().Where(predicate).Max(field) : 0;

    }
}
