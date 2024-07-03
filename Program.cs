using Microsoft.EntityFrameworkCore;
using NetAtlas_The_True_Project.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvcCore().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<NetAtlasDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("netAtlasContext")));
//Pour les sessions
builder.Services.AddSession(options =>{
    options.IdleTimeout = TimeSpan.FromMinutes(15);
});
builder.Services.AddMvc().AddJsonOptions(option =>
{
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Internaute}/{action=SignIn}/{id?}");

app.Run();
