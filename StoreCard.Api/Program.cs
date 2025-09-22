using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StoreCard.Application;
using StoreCard.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add DbContext from Data project
        builder.Services.AddDbContext<StoreCardDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "StoreCard API",
                Description = "An ASP.NET Core Web API for managing Users and Transaction",
                TermsOfService = new Uri("https://storecard.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Contact",
                    Email = "getjose@gmail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "License",
                    Url = new Uri("https://jo2web.com")
                }
            });

            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        });

        builder.Services.RegisterDataRepository(builder.Configuration);
        builder.Services.RegisterApplicationServices();

        var app = builder.Build();

        //Apply migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreCardDbContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreCard API v1");
                option.RoutePrefix = string.Empty;
            });
        }
        else
        {
            app.UseExceptionHandler("/error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}