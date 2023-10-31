using Cafeteria.Data;
using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Cafeteria;
using Microsoft.AspNetCore.Builder;
//using Cafeteria.Services.Interfaces;
//using Cafeteria.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

builder.Services.AddDbContext<CafeteriaContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("CafeteriaContext")));

//builder.Services.AddScoped<IProdutoService, ProdutoService>();
//builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddHsts(options => {
    options.MaxAge = TimeSpan.FromDays(30);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
