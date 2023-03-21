namespace SE1611_Group_4_Final_Project.Models
{
    public partial class Room
    {
        public Room()
        {
            RoomFurnitures = new HashSet<RoomFurniture>();
            Invoices = new HashSet<Invoice>();
            Notifications = new HashSet<Notification>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Area { get; set; }
        public string Address { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<RoomFurniture> RoomFurnitures { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
