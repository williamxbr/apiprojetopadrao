using Projeto.Padrao.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Padrao.Repository.Repository
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        public void Adicionar(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task AdicionarAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Buscar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> BuscarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> BuscarTodosAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Consultar(Func<TEntity, bool> lambda)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> ConsultarAsync(Func<TEntity, bool> lambda)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Remover(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoverAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
