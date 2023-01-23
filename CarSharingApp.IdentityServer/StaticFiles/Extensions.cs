using CarSharingApp.Domain.Entities;
using CarSharingApp.IdentityServer.Storages;
using CarSharingApp.Infrastructure.MongoDB;
using Serilog;

namespace CarSharingApp.IdentityServer.StaticFiles
{
    public static class Extensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Auth/SignIn";

                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
                //.AddInMemoryClients(IdentityServerConfigurations.GetClients())
                .AddClientStore<CustomersStorage>()
                .AddInMemoryApiScopes(IdentityServerConfigurations.GetApiScopes())
                .AddInMemoryIdentityResources(IdentityServerConfigurations.GetIdentityResources())
                .AddDeveloperSigningCredential();

            builder.Services.AddMongo(builder.Configuration);
            builder.Services.AddMongoRepository<Customer>(builder.Configuration["MongoDbConfig:Collections:CustomersCollectionName"] ?? "");

            builder.Services.AddControllersWithViews();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.MapDefaultControllerRoute();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            return app;
        }
    }
}
