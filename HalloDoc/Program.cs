using HalloDoc.Entity.Models;
using Microsoft.EntityFrameworkCore;
using HalloDoc.Repository.Interface;
using HalloDoc.Repository.Implement;
using HalloDoc.Entity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HalloDocDbContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("HalloDocDb"))
);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<IGenral, GenralRepo>();
builder.Services.AddScoped<IPatient, PatientRepo>();
builder.Services.AddScoped<IPatient, PatientRepo>();
builder.Services.AddScoped<IAdmin, AdminRepo>();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=PatientSite}/{id?}");

app.Run();
