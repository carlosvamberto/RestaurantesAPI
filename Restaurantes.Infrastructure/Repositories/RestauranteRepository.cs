using Microsoft.EntityFrameworkCore;
using Restaurantes.Domain.Entities;
using Restaurantes.Domain.Interfaces;
using Restaurantes.Infrastructure.Context;

namespace Restaurantes.Infrastructure.Repositories
{
    public class RestauranteRepository : IRestauranteRepository
    {
        private readonly MyDbContext _context;

        public RestauranteRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurante>> GetFilteredAsync(string? nome, string? tipo, string? cidade)
        {
            var query = _context.Restaurantes.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(r => r.Nome.Contains(nome));
            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(r => r.Tipo.Contains(tipo));
            if (!string.IsNullOrEmpty(cidade))
                query = query.Where(r => r.Cidade.Contains(cidade));

            var listResult = await query.ToListAsync();

            return listResult;
        }
    }
}
