namespace Catalog.DataAccess.DTO
{
    public class CategoryDal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public int? ParentId { get; set; }
        public CategoryDal? Parent { get; set; }
    }
}
