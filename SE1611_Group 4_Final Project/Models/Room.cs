using System;
using System.Collections.Generic;

namespace SE1611_Group_4_Final_Project.Models
{
    public partial class Room
    {
        public Room()
        {
            Invoices = new HashSet<Invoice>();
            RoomFurnitures = new HashSet<RoomFurniture>();
            Users = new HashSet<User>();
            Notifications = new HashSet<Notification>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Area { get; set; }
        public string Address { get; set; } = null!;
        public bool? IsAvailable { get; set; }

        public virtual BookingRequest? BookingRequest { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<RoomFurniture> RoomFurnitures { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
