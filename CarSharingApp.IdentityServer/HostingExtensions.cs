using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.Services;
using CarSharingApp.Domain.Entities;
using CarSharingApp.IdentityServer.Storages;
using CarSharingApp.Infrastructure.MongoDB;
using Serilog;

namespace CarSharingApp.IdentityServer
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            // uncomment if you want to add a UI
            //builder.Services.AddRazorPages();

            builder.Services.AddMongo(builder.Configuration);
            builder.Services.AddMongoRepository<Customer>(builder.Configuration["MongoDbConfig:Collections:CustomersCollectionName"] ?? "");
            builder.Services.AddSingleton<ICustomerService, CustomerService>();

            builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddClientStore<CustomersStorage>();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add a UI
            //app.UseStaticFiles();
            //app.UseRouting();

            app.UseIdentityServer();

            // uncomment if you want to add a UI
            //app.UseAuthorization();
            //app.MapRazorPages().RequireAuthorization();

            return app;
        }
    }
}
