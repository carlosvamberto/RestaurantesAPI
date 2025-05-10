namespace Restaurantes.Domain.Entities
{
    public class Restaurante
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Regiao { get; set; }
        public string Pais { get; set; }
    }
}
