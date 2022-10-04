using Lucene.Infrastructure.pojo;
using Lucene.Infrastructure.Repositories;
using Lucene.Logic.Interfaces;
using Lucene.Logic.Options;
using Lucene.Logic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton(p =>
{
    var config = new IndexOption();
    config.IndexFileFolder = builder.Configuration.GetValue<string>("IndexFileFolder");
    return config;
});

builder.Services.AddScoped<NewsService>();
builder.Services.AddScoped<INewsRepository, MockNewsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
