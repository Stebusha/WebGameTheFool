using BlazorCardGame.Components;
using BlazorCardGame.Models;
using BlazorCardGame.Services;
using BlazorCardGame.Contexts;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRadzenComponents();

builder.Services.AddAuthentication(GameConstants.AUTH_SCHEME)
    .AddCookie(GameConstants.AUTH_SCHEME, options =>
    {
        options.Cookie.Name = GameConstants.AUTH_COOKIE;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.LogoutPath = "/logout";

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;

        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
    });

// builder.Services.AddScoped<JSRuntime>();
builder.Services.AddScoped<FoolGameService>();
builder.Services.AddScoped<FoolDataManager>();
builder.Services.AddScoped<DataExportService>();

builder.Services.AddDbContextFactory<FoolGameContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("FoolGameConnection"), new MySqlServerVersion("8.4.3"));
    options.EnableSensitiveDataLogging();
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication()
    .UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
