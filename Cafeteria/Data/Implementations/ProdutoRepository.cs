using Cafeteria.Data;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Cafeteria.Services.Implementations
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CafeteriaContext _context;

        public ProdutoRepository(CafeteriaContext context)
        {
            _context = context;
        }

        public void Add(Produto produto)
        {
            try
            {
                _context.Produtos.Add(produto);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }
        public void Update(int id, Produto produto)
        {
            try
            {
                var produtoEncontrado = _context.Produtos.FirstOrDefault(x => x.Id == id);
                if (produtoEncontrado == null)
                {
                    return;
                }
                produtoEncontrado.Nome = produto.Nome;
                produtoEncontrado.Preco = produto.Preco;
                produtoEncontrado.Descricao = produto.Descricao;
                produtoEncontrado.Imagem = produto.Imagem;

                _context.Produtos.Update(produtoEncontrado);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }

        }
        public void Delete(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);
                if (produto != null)
                {
                    _context.Produtos.Remove(produto);
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }
        public bool Exists(int id)
        {
            return _context.Produtos.First(x => x.Id == id) != null;
        }
        public Produto Get(int id)
        {
            try
            {
                return _context.Produtos.First(x => x.Id == id);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }

        }
        public IEnumerable<Produto> GetAll()
        {
            try
            {
                List<Produto> list = new List<Produto>();
                foreach(var produto in _context.Produtos)
                {
                    if(String.IsNullOrEmpty(produto.Imagem))
                    {
                        // pegar imagem padrão sem-imagem.png
                        produto.Imagem = "sem-imagem.png";
                    }
                    list.Add(produto);
                }
                return list;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }

        }

        public IEnumerable<Produto> GetNome(string nome)
        {
            foreach (var produto in _context.Produtos)
            {
                produto.Nome = RemoveDiacritics(produto.Nome);
                nome = RemoveDiacritics(nome);
                if (produto.Nome.Contains(nome))
                {
                    yield return produto;
                }
            }
            //return _context.Produtos.Where(x => x.Nome.Contains(nome));
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
