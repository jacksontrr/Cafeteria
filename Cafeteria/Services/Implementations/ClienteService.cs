using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;
using Cafeteria.Utilities;

namespace Cafeteria.Services.Implementations
{
    public class ClienteService : IClienteService
    {

        private readonly IClienteRepository _clienteRepository;
        private readonly IFavoritoRepository _favoritoRepository;

        public ClienteService(IClienteRepository clienteRepository, IFavoritoRepository favoritoRepository)
        {
            _clienteRepository = clienteRepository;
            _favoritoRepository = favoritoRepository;
        }

        #region CRUD
        public async Task Add(Cliente cliente)
        {
            await _clienteRepository.Add(cliente);
        }

        public async Task Update(int id, Cliente cliente)
        {
            await _clienteRepository.Update(id, cliente);
        }

        public async Task Delete(int id)
        {
            bool exists = Exists(id);
            if (!exists)
            {
                return;
            }
            await _clienteRepository.Delete(id);
        }

        public async Task<IEnumerable<Cliente>> GetAll()
        {
            return await _clienteRepository.GetAll();
        }

        #endregion

        public async Task<IEnumerable<Cliente>> GetNome(string nome)
        {
            List<Cliente> list = new List<Cliente>();

            foreach (var item in await _clienteRepository.GetAll())
            {
                string nomeDB = CharacterTreatment.RemoveDiacritics(item.Nome).ToLower();
                nome = CharacterTreatment.RemoveDiacritics(nome).ToLower();
                if (nomeDB.Contains(nome))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public async Task<Cliente> Get(int id)
        {
            return await _clienteRepository.Get(id);
        }

        public async Task SaveFavorite(Favorito favorito)
        {
            await _favoritoRepository.Add(favorito);
        }

        public async Task<Favorito> GetFavoritoById(int id)
        {
            return await _favoritoRepository.Get(id);
        }

        public async Task DeleteFavorito(int id)
        {
            await _favoritoRepository.Delete(id);
        }

        public async Task<IEnumerable<Favorito>> GetClienteAll(int clienteId)
        {
            return await _favoritoRepository.GetClienteAll(clienteId);
        }

        public bool Exists(int id)
        {
            return _clienteRepository.Exists(id);
        }


    }
}
