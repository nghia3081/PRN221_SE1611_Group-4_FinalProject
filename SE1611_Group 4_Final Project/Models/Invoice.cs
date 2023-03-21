namespace SE1611_Group_4_Final_Project.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            Rooms = new HashSet<Room>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal GrandTotal { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string? Description { get; set; }
        public Guid? UserId { get; set; }
        public int? Status { get; set; }
        public int? Type { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public virtual User? User { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
