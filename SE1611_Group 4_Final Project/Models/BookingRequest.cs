namespace SE1611_Group_4_Final_Project.Models
{
    public partial class BookingRequest
    {
        public Guid RoomId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string IdentifyNumber { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool Accept { get; set; }
        public virtual Room Room { get; set; } = null!;
    }
}
