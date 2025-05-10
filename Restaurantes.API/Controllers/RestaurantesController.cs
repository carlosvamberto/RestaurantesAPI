using Microsoft.AspNetCore.Mvc;
using Restaurantes.Application.Requests;
using Restaurantes.Application.Services;
using Restaurantes.Domain.Entities;

namespace Restaurantes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantesController : ControllerBase
    {
        private readonly IRestauranteService _restauranteService;

        public RestaurantesController(IRestauranteService restauranteService)
        {
            _restauranteService = restauranteService;
        }

        /// <summary>
        /// Lista restaurantes com base nos filtros fornecidos (Nome, Tipo e Cidade).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurante>>> Get(
            [FromQuery] string? nome,
            [FromQuery] string? tipo,
            [FromQuery] string? cidade)
        {
            var request = new GetRestaurantesRequest
            {
                Nome = nome,
                Tipo = tipo,
                Cidade = cidade
            };

            var result = await _restauranteService.GetFilteredAsync(request);

            return Ok(result);
        }
    }
}
