namespace Play.Catalog.Service.Entities
{
    public class Item
    {
        // [Key] is not needed because EF Core will automatically assume that the Id property is the primary key of the table.
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // [Column(TypeName = "decimal(18,2)")] is not needed because EF Core will automatically assume that the Price property is a decimal(18,2) column.
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}