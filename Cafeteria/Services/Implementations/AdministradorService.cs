using Cafeteria.Data;
using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;

namespace Cafeteria.Services.Implementations
{
    public class AdministradorService : IAdministradorService
    {
        
        private readonly IAdministradorRepository _administradorRepository;
        public AdministradorService(IAdministradorRepository administradorRepository)
        {
            _administradorRepository = administradorRepository;
        }

        #region CRUD
        public async Task Add(Administrador administrador)
        {
            await _administradorRepository.Add(administrador);
        }

        public async Task Delete(int id)
        {
            await _administradorRepository.Delete(id);
        }

        public async Task Update(int id, Administrador administrador)
        {
            await _administradorRepository.Update(id, administrador);
        }

        public async Task<IEnumerable<Administrador>> GetAll()
        {
            return await _administradorRepository.GetAll();
        }

        #endregion

        public async Task<Administrador> Get(int id)
        {
            return await _administradorRepository.Get(id);
        }

        public async Task<IEnumerable<Administrador>> GetNome(string nome)
        {
           return await _administradorRepository.GetNome(nome);
        }

        public async Task<Administrador> GetEmail(string email)
        {
            return await _administradorRepository.GetEmail(email);
        }

        public bool Exists(int id)
        {
            return _administradorRepository.Exists(id);
        }
    }
}
