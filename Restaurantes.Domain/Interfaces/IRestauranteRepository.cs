using Restaurantes.Domain.Entities;

namespace Restaurantes.Domain.Interfaces
{
    public interface IRestauranteRepository
    {
        Task<IEnumerable<Restaurante>> GetFilteredAsync(string? nome, string? tipo, string? cidade);
    }
}
