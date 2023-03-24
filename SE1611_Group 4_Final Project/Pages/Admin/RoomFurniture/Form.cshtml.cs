using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Admin.RoomFurniture
{
    public class FormModel : PageModel
    {
        private readonly IRepository<Models.RoomFurniture> _context;
        [BindProperty]
        public Models.RoomFurniture RoomFurniture { get; set; }
        [BindProperty]
        public IFormFileCollection FormFiles { get; set; }
        public SelectList statusSelect { get; set; }
        public SelectList roomSelect { get; set; }
        private Dictionary<int, string> listStatus { get; set; }
        public FormModel(IRepository<Models.RoomFurniture> context)
        {
            _context = context;
        }
        public void OnGet(Guid? id)
        {
            roomSelect = new SelectList(_context.GetDbSet<Models.Room>().AsEnumerable(), "Id", "Name");

            listStatus = Enum.GetValues(typeof(Constant.RoomFurnitureStatus)).Cast<Constant.RoomFurnitureStatus>().ToDictionary(t => (int)t, t => t.ToString());
            statusSelect = new SelectList(listStatus.AsEnumerable(), "Key", "Value");
            if (id.HasValue)
            {
                RoomFurniture = _context.Find(id);
                roomSelect = new SelectList(_context.GetDbSet<Models.Room>().AsEnumerable(), "Id", "Name", RoomFurniture.RoomId);
                statusSelect = new SelectList(listStatus.AsEnumerable(), "Key", "Value", RoomFurniture.Status);
            }

        }
        public IActionResult OnPost()
        {
            if (RoomFurniture.Id == Guid.Empty)
            {
                RoomFurniture.Id = Guid.NewGuid();
                _context.Add(RoomFurniture);
            }
            else
            {
                _context.Update(RoomFurniture);
            }
            if (FormFiles.Count > 0)
            {
                UploadRoomImage(RoomFurniture.Id);
            }
            return RedirectToPage("/Admin/RoomFurniture/Index");
        }
        private async Task UploadRoomImage(Guid roomId)
        {
            if (FormFiles.Count == 0) return;
            await Task.Run(() =>
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img");
                directory = $"{directory}/roomfurniture/{roomId}";
                System.IO.Directory.CreateDirectory(directory);
                var i = 1;
                foreach (var file in Directory.GetFiles(directory))
                {
                    System.IO.File.Delete(file);
                }

                foreach (var file in FormFiles)
                {
                    using MemoryStream ms = new();
                    file.CopyTo(ms);
                    UploadAnImage(ms.ToArray(), directory, roomId, i);
                    i++;
                }
            });
        }
        private static async Task UploadAnImage(byte[] image, string directory, Guid roomId, int index)
        {
            await System.IO.File.WriteAllBytesAsync($"{directory}/{roomId}-{index}.png", image);
        }
    }
}
