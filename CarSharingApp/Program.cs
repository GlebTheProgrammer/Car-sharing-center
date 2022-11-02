using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Repository.LocalRepository;
using CarSharingApp.Services;
using CarSharingApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Sessions setting
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Session time regulates here
});

// Connect AutoMapper in the program
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency injection is here
builder.Services.AddSingleton<IVehiclesRepository, VehiclesLocalRepository>();
builder.Services.AddSingleton<IClientsRepository, ClientsLocalRepository>();
builder.Services.AddSingleton<IOrdersRepository, OrdersLocalRepository>();
builder.Services.AddSingleton<IRatingRepository, RatingLocalRepository>();

builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICurrentUserStatusProvider, CurrentUserStatusProviderService>();

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

// Here we can specify the start page wich is shown to the user
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
