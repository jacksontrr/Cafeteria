using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Data.Implementations
{
    public class FormaPagamentoRepository : IFormaPagamentoRepository
    {
        private readonly CafeteriaContext _context;

        public FormaPagamentoRepository(CafeteriaContext context)
        {
            _context = context;
        }

        #region CRUD
        public async Task Add(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Pagamento pagamento)
        {
            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if(pagamento != null)
            {
                _context.Pagamentos.Remove(pagamento);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Pagamento>> GetAll()
        {
            var pagamento = await _context.Pagamentos.ToListAsync();
            if(pagamento != null)
            {
                return pagamento;
            }
            return null;
        }

        public async Task<Pagamento> Get(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if(pagamento != null)
            {
                return pagamento;
            }
            return null;
        }

        #endregion
    }
}
