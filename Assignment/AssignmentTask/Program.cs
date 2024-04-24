using AssignmentTask.Entity.Data;
using AssignmentTask.Repository.Implement;
using AssignmentTask.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryDbContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("LibraryDb"))
);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<ILibrary, Library>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Library}/{action=LibraryHome}/{id?}");

app.Run();
