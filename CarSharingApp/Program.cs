using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Repository.LocalRepository;
using CarSharingApp.Services;
using CarSharingApp.Services.Interfaces;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Sessions setting
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Dependency injection is here
builder.Services.AddSingleton<IRepositoryManager, LocalRepositoryManager>();

builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICurrentUserStatusProvider, CurrentUserStatusProviderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

StripeConfiguration.ApiKey = "sk_test_51M6B0AGBXizEWSwDh5mkyk4o3DvKzmywGwJh7Fg2cpd9mxmhLiIPkARsFcvN3Yov0Qyshlqu8gITm3NGPPReXtbW00dvIu6aGa";

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
