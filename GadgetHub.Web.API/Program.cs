using GadgetHub.Application;
using GadgetHub.Application.Common.Mappings;
using GadgetHub.Infrastructure;
using GadgetHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;


namespace GadgetHub.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            
           
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GadgetHub API",
                    Version = "v1",
                    Description = "API for GadgetHub Product Catalog Management"
                });
            });

            // AutoMapper setup
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(typeof(MappingProfile));
            });

            // EF Core DbContext setup
            builder.Services.AddDbContext<MyDBcontext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("constr")));

            // Register Application and Infrastructure layers
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GadgetHub API v1");
                    c.RoutePrefix = "swagger"; // Accessible at /swagger
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}