using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

using TaskManagement.Core.Interfaces;
using TaskManagement.Data;
using TaskManagement.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=data.db");
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
