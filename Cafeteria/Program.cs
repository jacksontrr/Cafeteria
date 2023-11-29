using Cafeteria.Data;
using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Cafeteria;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Cafeteria.Services.Interfaces;
using Cafeteria.Services.Implementations;
using Cafeteria.Data.Repositories;
using Cafeteria.Data.Implementations;
using Cafeteria.Utilities;
using Microsoft.AspNetCore.WebSockets;
using System.Net.WebSockets;
using System.Text;
using System.Globalization;

//List<WebSocket> _sockets = new List<WebSocket>();

var builder = WebApplication.CreateBuilder(args);
var cultureInfo = new CultureInfo("pt-BR");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddDbContext<CafeteriaContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("CafeteriaContext")));
builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

#region Create User Admin
var userAdmin = builder.Configuration["UserAdmin"];
var emailAdmin = builder.Configuration["EmailAdmin"];
var passwordAdmin = builder.Configuration["PasswordAdmin"];

if (!string.IsNullOrEmpty(userAdmin) && !string.IsNullOrEmpty(emailAdmin) && !string.IsNullOrEmpty(passwordAdmin))
{
    var connectionString = builder.Configuration.GetConnectionString("CafeteriaContext");

    if (!string.IsNullOrEmpty(connectionString))
    {
        var optionsBuilder = new DbContextOptionsBuilder<CafeteriaContext>();
        optionsBuilder.UseSqlite(connectionString);

        using var context = new CafeteriaContext(optionsBuilder.Options);

        var existingAdmin = context.Administradores.FirstOrDefault(a => a.Email == emailAdmin);
        var existingClient = context.Clientes.FirstOrDefault(c => c.Email == emailAdmin);
        passwordAdmin = PasswordUtilities.PasswordHash(passwordAdmin);
        if (existingAdmin == null && existingClient == null)
        {
            context.Administradores.Add(new Administrador
            {
                Nome = userAdmin,
                Email = emailAdmin,
                Senha = passwordAdmin
            });

            context.SaveChanges(); // Salva as alterações no banco de dados
        }
    }
}
#endregion

#region Repositories
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IFavoritoRepository, FavoritoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IFormaPagamentoRepository, FormaPagamentoRepository>();
#endregion

#region Services
builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddSingleton<ICarrinhoService, CarrinhoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IFormaPagamentoService, FormaPagamentoService>();
#endregion

#region Utilities
#endregion

builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(30);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = true;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Login/Index"; // Defina a página de login
    options.AccessDeniedPath = "/Login/AcessoNegado"; // Defina a página de acesso negado
});

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LastRequestMiddleware>();

app.UseWebSockets();
app.UseMiddleware<Cafeteria.Utilities.WebSocketMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Produtos}/{action=Index}/{id?}"
    );

});


app.Run();
