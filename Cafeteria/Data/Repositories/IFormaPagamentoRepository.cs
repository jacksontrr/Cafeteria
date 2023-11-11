using Cafeteria.Models;

namespace Cafeteria.Data.Repositories
{
    public interface IFormaPagamentoRepository
    {
        #region CRUD
        Task Add(Pagamento pagamento);
        Task Update(int id, Pagamento pagamento);
        Task Delete(int id);
        Task<IEnumerable<Pagamento>> GetAll();
        Task<Pagamento> Get(int id);
        #endregion
    }
}
