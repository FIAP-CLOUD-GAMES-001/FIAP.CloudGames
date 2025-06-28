using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Domain.Interfaces.Repositories.Base
{
    public interface IRepositoryBase <TEntity> : IDisposable where TEntity : class
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        bool Exists(Func<TEntity, bool> where);
        IQueryable<TEntity> List(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> ListWhere(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> ListWhereAsNoTracking(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetBy(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] includeProperties);
        
    }
}
