using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Padrao.Domain.Interfaces.Repository
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        void Adicionar(TEntity entity);
        void Atualizar(TEntity entity);
        void Remover(TEntity entity);
        TEntity Buscar(int id);
        IEnumerable<TEntity> BuscarTodos();
        IQueryable<TEntity> Consultar(Func<TEntity, bool> lambda);
        void Dispose();

        Task AdicionarAsync(TEntity entity);
        Task AtualizarAsync(TEntity entity);
        Task RemoverAsync(TEntity entity);
        Task<TEntity> BuscarAsync(int id);
        Task<IEnumerable<TEntity>> BuscarTodosAsync();
        Task<IQueryable<TEntity>> ConsultarAsync(Func<TEntity, bool> lambda);

    }
}
