using Cafeteria.Data;
using Cafeteria.Models;
using Cafeteria.Services.Interfaces;
using Cafeteria.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        #region CRUD
        public async Task Add(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Produto produto)
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
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Produto>> GetAll(int? clienteId)
        {
            // Iniciar a lista de produtos
            List<Produto> produtos;

            if (clienteId.HasValue)
            {
                produtos = await _context.Produtos
                                        .Include(p => p.Favoritos.Where(f => f.ClienteId == clienteId))
                                        .Distinct() // Isso garante que os produtos não estejam duplicados
                                        .ToListAsync();
            }
            else
            {
                produtos = await _context.Produtos.ToListAsync();
            }

            foreach (var produto in produtos)
            {
                if (String.IsNullOrEmpty(produto.Imagem))
                {
                    produto.Imagem = "sem-imagem.png";
                }
            }

            return produtos;
        }

        public async Task<Produto> Get(int id)
        {
            Produto produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
            if (String.IsNullOrEmpty(produto.Imagem))
            {
                produto.Imagem = "sem-imagem.png";
            }
            return produto;
        }

        #endregion

        public async Task<IEnumerable<Produto>> GetNome(string nome)
        {
            List<Produto> list = new List<Produto>();
            foreach (var produto in await _context.Produtos.ToListAsync())
            {
                string nomeBD = produto.Nome;
                nomeBD = CharacterTreatment.RemoveDiacritics(nomeBD);
                nome = CharacterTreatment.RemoveDiacritics(nome);
                if (nomeBD.ToUpper().Contains(nome.ToUpper()))
                {
                    list.Add(produto);
                }
            }
            return list;
        }

        public bool Exists(int id)
        {
            return _context.Produtos.Any(x => x.Id == id);
        }

    }
}
