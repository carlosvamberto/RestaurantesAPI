// Importa o serviço de cache distribuído da Microsoft (usado para Redis, por exemplo)
using Microsoft.Extensions.Caching.Distributed;

// Importa a classe de requisição que contém os filtros (Nome, Tipo, Cidade)
using Restaurantes.Application.Requests;

// Importa a interface do serviço que esta classe implementa
using Restaurantes.Application.Services;

// Importa a entidade Restaurante
using Restaurantes.Domain.Entities;

// Importa a interface do repositório que acessa o banco de dados
using Restaurantes.Domain.Interfaces;

// Usado para serializar e desserializar objetos para armazenar no cache
using System.Text.Json;

// Define o namespace do projeto onde o serviço está localizado
namespace Restaurantes.Infrastructure.Services
{
    // Implementação concreta da interface IRestauranteService
    public class RestauranteService : IRestauranteService
    {
        // Injeção de dependência do repositório de restaurantes
        private readonly IRestauranteRepository _repository;
        // Injeção de dependência do cache distribuído (como o Redis)
        private readonly IDistributedCache _cache;

        // Construtor que recebe as dependências via injeção
        public RestauranteService(IRestauranteRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        // Método assíncrono que retorna uma lista de restaurantes filtrados
        public async Task<IEnumerable<Restaurante>> GetFilteredAsync(GetRestaurantesRequest request)
        {
            // Gera uma chave única para o cache com base nos filtros recebidos
            var cacheKey = $"restaurantes:{request.Nome}:{request.Tipo}:{request.Cidade}";

            // Tenta recuperar os dados do cache usando a chave gerada
            var cached = await _cache.GetStringAsync(cacheKey);

            // Se encontrou dados no cache, desserializa e retorna a lista de restaurantes
            if (cached != null && cached != "[]")
            {
                return JsonSerializer.Deserialize<IEnumerable<Restaurante>>(cached)!;
            }

            // Caso não tenha no cache, consulta o repositório (acessa o banco de dados)
            var result = await _repository.GetFilteredAsync(request.Nome, request.Tipo, request.Cidade);

            // Serializa o resultado em JSON para armazenar no cache
            var json = JsonSerializer.Serialize(result);

            // Armazena os dados no cache com uma expiração de 5 minutos
            await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });


            return result;
        }
    }

}
