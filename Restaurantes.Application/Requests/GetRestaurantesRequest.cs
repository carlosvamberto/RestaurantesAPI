namespace Restaurantes.Application.Requests
{
    public class GetRestaurantesRequest
    {
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public string? Cidade { get; set; }
    }
}
