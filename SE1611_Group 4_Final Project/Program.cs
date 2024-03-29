using SE1611_Group_4_Final_Project.Utils;
using SE1611_Group_4_Final_Project.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.RegisterMyServices();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<ChatHub>("/chatHub");
});
app.UseMvc();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();