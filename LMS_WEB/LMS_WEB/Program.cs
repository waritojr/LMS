using LMS_WEB.Interfaces;
using LMS_WEB.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IUserModel, UserModel>();
builder.Services.AddSingleton<IBookModel, BookModel>();
builder.Services.AddSingleton<IReservationModel, ReservationModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var supportredCultures = new[]
{
    new CultureInfo("es-CR")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("es-CR"),
    SupportedCultures = supportredCultures,
    SupportedUICultures = supportredCultures

});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
