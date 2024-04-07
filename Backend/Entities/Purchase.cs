namespace Backend.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}