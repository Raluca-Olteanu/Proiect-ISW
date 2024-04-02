namespace Backend.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}