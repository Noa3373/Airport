using Airport.Client;
using Airport.Client.Hubs;
using Airport.Client.Services;
using Airport.Data;
using Airport.Data.Models;
using Airport.Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AirportDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("AirportDdContext")));
builder.Services.AddScoped<IRepository<Flight>, FlightRepository>();
builder.Services.AddScoped<IRepository<Terminal>, TerminalRepository>();
builder.Services.AddScoped<IRepository<Airplane>, AirplaneRepository>();
builder.Services.AddScoped<FlightService>();
//builder.Services.AddHostedService<RestartService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSignalR();
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<FlightHub>("/flightHub");
});

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Airport}/{action=Index}/{id?}");

app.Run();
