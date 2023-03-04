﻿namespace SE1611_Group_4_Final_Project.Models
{
    public partial class RoomFurniture : Entity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int? StatusId { get; set; }
        public Guid? RoomId { get; set; }

        public virtual Room? Room { get; set; }
        public virtual FurnitureStatus? Status { get; set; }
    }
}
