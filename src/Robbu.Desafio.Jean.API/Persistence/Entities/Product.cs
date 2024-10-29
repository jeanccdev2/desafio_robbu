namespace Robbu.Desafio.Jean.API.Persistence.Entities
{
    public class Product
    {
        public int Id { get; init; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }
        
        public bool? IsDeleted { get; set; }
    }
}