using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicCloud.Application.infrastructure;
using MusicCloud.Application.Infrastructure;
using MusicCloud.Application.Infrastructure.Repositories;
using MusicCloud.Application.Model;
using MusicCloud.Webapp.Dto;
using MusicCloud.Webapp.Services;

var builder = WebApplication.CreateBuilder(args);

//Creating and seeding the database
var opt = new DbContextOptionsBuilder()
    .UseSqlite("DataSource=musiccloud.db")
    .Options;
using (var db = new CloudContext(opt))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed(new CryptService());
}

// Add services to the container.
builder.Services.AddDbContext<CloudContext>(opt =>
{
    opt.UseSqlite("DataSource=musiccloud.db");
});
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<ArtistRepository>();
builder.Services.AddTransient<AlbumRepository>();
builder.Services.AddTransient<SongRepository>();
builder.Services.AddTransient<GenreRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICryptService, CryptService>();
builder.Services.AddTransient<AuthService>(provider => new AuthService(
    isDevelopment: builder.Environment.IsDevelopment(),
    db: provider.GetRequiredService<CloudContext>(),
    crypt: provider.GetRequiredService<ICryptService>(),
    httpContextAccessor: provider.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddAuthentication(
    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/User/Login";
        o.AccessDeniedPath = "/User/AccessDenied";
    });
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("ManagerOrAdminRole", p => p.RequireRole(Usertype.Manager.ToString(), Usertype.Admin.ToString()));
});


var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
