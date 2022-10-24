using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Repository.LocalRepository;
using CarSharingApp.Services;
using CarSharingApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Connect AutoMapper in the program
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency injection is here
builder.Services.AddSingleton<IVehiclesRepository, VehiclesLocalRepository>();
builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>();

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

// Here we can specify the start page wich is shown to the user
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
