using Application;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CertiFlowAPI",
        Description = "Open API for CertiFlow web application",
        Contact = new OpenApiContact
        {
            Email = "vadym.radchuk@oa.edu.ua",
            Name = "Vadym Radchuk"
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.WithOrigins("http://localhost:5173") 
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Configure HttpClient for Google APIs
builder.Services.AddHttpClient("GoogleOAuth", client =>
{
    client.BaseAddress = new Uri("https://oauth2.googleapis.com/");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("GoogleApi", client =>
{
    client.BaseAddress = new Uri("https://www.googleapis.com/");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region ContextSeed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<DataContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DataSeeder.SeedDataAsync(context, roleManager);
        DbInitializer.Seed(context);

    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
        logger.LogError(ex.Message, "Не вдалося ініціалізувати та заповнити базу даних");
    }
}
#endregion

app.Run();