using System;
using System.Collections.Generic;

namespace SE1611_Group_4_Final_Project.Models
{
    public partial class RoomFurniture
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Status { get; set; }
        public Guid? RoomId { get; set; }

        public virtual Room? Room { get; set; }
    }
}
