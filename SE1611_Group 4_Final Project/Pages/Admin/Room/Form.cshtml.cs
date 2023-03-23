using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages.Room
{
    public class FormModel : PageModel
    {
        private readonly IRepository<Models.Room> _context;
        [BindProperty]
        public Models.Room Room { get; set; }
        [BindProperty]
        public IFormFileCollection FormFiles { get; set; }
        public FormModel(IRepository<Models.Room> context)
        {
            _context = context;
        }
        public void OnGet(Guid? id)
        {
            if (id.HasValue)
            {
                Room = _context.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            if (Room.Id == Guid.Empty)
            {
                Room.Id = Guid.NewGuid();
                _context.Add(Room);
            }
            else
            {
                _context.Update(Room);
            }
            if (FormFiles.Count > 0)
            {
                UploadRoomImage(Room.Id);
            }
            return RedirectToPage("/Admin/Room/Index");
        }
        private async Task UploadRoomImage(Guid roomId)
        {
            if (FormFiles.Count == 0) return;
            await Task.Run(() =>
            {
                var directory = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img");
                 directory = $"{directory}/room/{roomId}";
                System.IO.Directory.CreateDirectory(directory);
                var i = 1;
                foreach(var file in Directory.GetFiles(directory))
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
