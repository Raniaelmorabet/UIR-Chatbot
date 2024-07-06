using System.Text.Json;
using System.Text.Json.Serialization;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OpenAI_App_UIR.Data;
//using OpenAI_UIR.Data;
//using OpenAI_UIR.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddDbContext<ConversationContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // Enable sensitive data logging
           .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())) // Enable console logging
);



// Configure OpenAIClient
builder.Services.AddSingleton<OpenAIClient>(sp =>
{
    var endpoint = new Uri(builder.Configuration["AzureOpenAI:Endpoint"]);
    var apiKey = builder.Configuration["AzureOpenAI:ApiKey"];
    return new OpenAIClient(endpoint, new AzureKeyCredential(apiKey));
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Use the CORS policy
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
