using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using System.Reflection;
//using FlightPlanApi.Authentication;
using FlightPlanApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<IDatabaseAdapter, MongoDbDatabase>(); // This means in the controller 
//now just reference IDatabaseAdapter so refactoring to a new database doesn't require a change in the controller
//what is addsingleton and is addscope better for me?

// Set up URLs that the app will listen on
builder.WebHost.UseUrls("http://localhost:3000", "https://localhost:3001");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// we want to restrict this
app.UseCors(options => options
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
