namespace Catalog.DataAccess.DTO
{
    public class ProductDal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }

        public int? CategoryId { get; set; }
        public CategoryDal? Category { get; set; }
    }
}
