namespace SE1611_Group_4_Final_Project.Models
{
    public partial class FurnitureStatus
    {
        public FurnitureStatus()
        {
            RoomFurnitures = new HashSet<RoomFurniture>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<RoomFurniture> RoomFurnitures { get; set; }
    }
}
