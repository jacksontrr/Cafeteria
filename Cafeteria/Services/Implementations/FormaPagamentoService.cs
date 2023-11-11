using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;

namespace Cafeteria.Services.Implementations
{
    public class FormaPagamentoService :IFormaPagamentoService
    {
        private readonly IFormaPagamentoRepository  _formaPagamentoRepository;

        public FormaPagamentoService(IFormaPagamentoRepository formaPagamentoRepository)
        {
            _formaPagamentoRepository = formaPagamentoRepository;
        }

        #region CRUD

        public async Task Add(Pagamento pagamento)
        {
            await _formaPagamentoRepository.Add(pagamento);
        }

        public async Task Update(int id, Pagamento pagamento)
        {
            await _formaPagamentoRepository.Update(id, pagamento);
        }

        public async Task Delete(int id)
        {
            await _formaPagamentoRepository.Delete(id);
        }

        public async Task<IEnumerable<Pagamento>> GetAll()
        {
            return await _formaPagamentoRepository.GetAll();
        }

        public async Task<Pagamento> Get(int id)
        {
            return await _formaPagamentoRepository.Get(id);
        }

        #endregion
    }
}
