
using Microsoft.OpenApi.Models;
using StoreCard.Application;
using StoreCard.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
        });

        builder.Services.RegisterApplicationServices();
        builder.Services.RegisterDataRepository();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}