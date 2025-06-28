using FIAP.CloudGames.Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.infrastructure.Repositories.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
         where TEntity : class
    {
        protected readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }
        public TEntity Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Exists(Func<TEntity, bool> where)
        {
            throw new NotImplementedException();
        }

        public TEntity GetBy(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> List(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> ListWhere(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> ListWhereAsNoTracking(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
