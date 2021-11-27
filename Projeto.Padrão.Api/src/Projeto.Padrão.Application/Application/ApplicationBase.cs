using Projeto.Padrao.Domain.Interfaces.Application;
using Projeto.Padrao.Domain.Interfaces.Repository;

namespace Projeto.Padrão.Application.Application
{
    public class ApplicationBase<TEntity> : IDisposable, IApplicationBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;

        public ApplicationBase(IRepositoryBase<TEntity> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public void Adicionar(TEntity entity)
        {
            _repositoryBase.Adicionar(entity);
        }

        public async Task AdicionarAsync(TEntity entity)
        {
            await _repositoryBase.AdicionarAsync(entity);
        }

        public void Atualizar(TEntity entity)
        {
            _repositoryBase.Atualizar(entity);
        }

        public async Task AtualizarAsync(TEntity entity)
        {
            await _repositoryBase.AtualizarAsync(entity);
        }

        public TEntity Buscar(int id)
        {
            return _repositoryBase.Buscar(id);
        }

        public async Task<TEntity> BuscarAsync(int id)
        {
            return await _repositoryBase.BuscarAsync(id);
        }

        public IEnumerable<TEntity> BuscarTodos()
        {
            return _repositoryBase.BuscarTodos();
        }

        public async Task<IEnumerable<TEntity>> BuscarTodosAsync()
        {
            return await _repositoryBase.BuscarTodosAsync();
        }

        public IQueryable<TEntity> Consultar(Func<TEntity, bool> lambda)
        {
            return _repositoryBase.Consultar(lambda);
        }

        public async Task<IQueryable<TEntity>> ConsultarAsync(Func<TEntity, bool> lambda)
        {
            return await _repositoryBase.ConsultarAsync(lambda);
        }

        public void Dispose()
        {
            _repositoryBase.Dispose();
        }

        public void Remover(TEntity entity)
        {
            _repositoryBase.Remover(entity);
        }

        public async Task RemoverAsync(TEntity entity)
        {
            await _repositoryBase.RemoverAsync(entity);
        }
    }
}
