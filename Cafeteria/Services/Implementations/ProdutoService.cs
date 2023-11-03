using Cafeteria.Data;
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

        public ProdutoService(IProdutoRepository produtoRepository, IWebHostEnvironment webHostEnvironment)
        {
            _produtoRepository = produtoRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Add(Produto produto)
        {
            _produtoRepository.Add(produto);
            _produtoRepository.SaveChanges();
        }

        public void Delete(int id)
        {
            _produtoRepository.Delete(id);
            _produtoRepository.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _produtoRepository.Exists(id);
        }

        public Produto Get(int id)
        {
            return _produtoRepository.Get(id);
        }

        public IEnumerable<Produto> GetAll()
        {
            return _produtoRepository.GetAll();
        }
        public IEnumerable<Produto> GetNome(string nome)
        {
            return _produtoRepository.GetNome(nome);
        }

        public void Update(int id, Produto produto)
        {
            _produtoRepository.Update(id, produto);
            _produtoRepository.SaveChanges();
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
    }
}
