using Cafeteria.Data;
using Cafeteria.Data.Repositories;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;
using Cafeteria.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace Cafeteria.Services.Implementations
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFavoritoRepository _favoritoRepository;

        public ProdutoService(IProdutoRepository produtoRepository, IWebHostEnvironment webHostEnvironment, IFavoritoRepository favoritoRepository)
        {
            _produtoRepository = produtoRepository;
            _webHostEnvironment = webHostEnvironment;
            _favoritoRepository = favoritoRepository;
        }

        #region CRUD
        public async Task Add(Produto produto)
        {
            await _produtoRepository.Add(produto);
        }

        public async Task Update(int id, Produto produto)
        {
            await _produtoRepository.Update(id, produto);
        }

        public async Task Delete(int id)
        {
            await _produtoRepository.Delete(id);
        }

        public async Task<IEnumerable<Produto>> GetAll(int? clienteId)
        {
            return await _produtoRepository.GetAll(clienteId);
        }

        #endregion

        public bool Exists(int id)
        {
            return _produtoRepository.Exists(id);
        }

        public async Task<Produto> Get(int id)
        {
            return await _produtoRepository.Get(id);
        }

        public async Task<IEnumerable<Produto>> GetNome(string nome)
        {
            return await _produtoRepository.GetNome(nome);
        }

        public bool SaveFile(ref ProdutoViewModel produtoViewModel)
        {
            bool error = false;
            if (produtoViewModel.Arquivo != null && produtoViewModel.Arquivo.Length > 0)
            {
                // Verifique se o tipo de arquivo é válido
                if (!produtoViewModel.Arquivo.ContentType.Contains("image"))
                {
                    error = true;
                    return error;
                }

                // Defina o diretório de destino para salvar o arquivo
                var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images/product");

                // Crie o diretório se ele não existir
                Directory.CreateDirectory(uploadsDirectory);

                // Gere um nome de arquivo exclusivo
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(produtoViewModel.Arquivo.FileName);

                // Combine o diretório e o nome do arquivo
                var filePath = Path.Combine(uploadsDirectory, fileName);

                // Salve o arquivo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    produtoViewModel.Arquivo.CopyToAsync(stream);
                }
                produtoViewModel.Imagem = fileName;
            }
            return error;
        }

        public void DeleteFile(string? fileName)
        {
            var caminhoDoArquivo = Path.Combine(_webHostEnvironment.WebRootPath, "images\\product", fileName);
            if (System.IO.File.Exists(caminhoDoArquivo))
            {
                System.IO.File.Delete(caminhoDoArquivo);
            }
        }

        public bool CheckIfItHasBeenChanged(Produto Antigo, Produto Novo)
        {
            bool verificar = false;

            if (Antigo.Nome != Novo.Nome)
            {
                verificar = true;
            }
            if (Antigo.Preco != Novo.Preco)
            {
                verificar = true;
            }
            if (Antigo.Descricao != Novo.Descricao)
            {
                verificar = true;
            }
            if (Antigo.Imagem != Novo.Imagem)
            {
                verificar = true;
            }
            return verificar;
        }

        public async Task SaveFavorite(Favorito favorito)
        {
            await _favoritoRepository.Add(favorito);
        }

        public async Task<Favorito> GetFavorito(Favorito favorito)
        {
            return await _favoritoRepository.GetClienteProduto(favorito);
        }

        public async Task DeleteFavorite(int id)
        {
            await _favoritoRepository.Delete(id);
        }
    }
}
