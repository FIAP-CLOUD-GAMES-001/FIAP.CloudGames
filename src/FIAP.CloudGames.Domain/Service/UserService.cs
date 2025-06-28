using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.CloudGames.Domain.Service
{
    public class UserService : IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public UserEntity Add(UserEntity entity)
        {
            if (_unitOfWork.UserRepository.GetBy(x => x.Email == entity.Email) != null)
                throw new ApplicationException("Usuário com esse email já existe.");

            //UserEntity entidadeUser = new UserEntity(entity); PASSARIA O DTO
            _unitOfWork.UserRepository.Add(entity);
            _unitOfWork.SaveChanges();
            return entity;

        }

        public void Delete(UserEntity entity)
        {
            UserEntity EmpresaDelete = _unitOfWork.UserRepository.GetBy(X => X.Id == entity.Id);
            _unitOfWork.UserRepository.Delete(EmpresaDelete);
            _unitOfWork.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Exists(Func<UserEntity, bool> where)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetBy(Func<UserEntity, bool> where, params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserEntity> List(params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            return _unitOfWork.UserRepository.List();
        }

        public IQueryable<UserEntity> ListWhere(Expression<Func<UserEntity, bool>> where, params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserEntity> ListWhereAsNoTracking(Expression<Func<UserEntity, bool>> where, params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public UserEntity Update(UserEntity entity)
        {
            UserEntity EmpresaExistente = _unitOfWork.UserRepository.GetBy(x =>x.Id == entity.Id);
            if (EmpresaExistente == null)
                throw new KeyNotFoundException("Empresa não encontrado.");

            // METODO EDITANDO EMPRESA \\
            // GERANDO MODELO EMPRESAEDITADA

            _unitOfWork.UserRepository.Update(EmpresaExistente);
            _unitOfWork.SaveChanges();
            return EmpresaExistente;
        }
    }
}
