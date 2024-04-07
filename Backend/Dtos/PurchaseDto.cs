namespace Backend.Dtos
{
    public class PurchaseDto
    {
        public int UserId { get; set; }
        public List<int> ProductIds { get; set; }
    }
}