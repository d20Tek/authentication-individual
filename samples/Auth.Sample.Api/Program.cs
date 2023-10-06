//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Auth.Sample.Api.Endpoints;
using D20Tek.Authentication.Individual;
using D20Tek.Authentication.Individual.Api;
using D20Tek.Minimal.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIndividualAuthentication(builder.Configuration);
builder.Services.AddAuthenticationApiEndpoints();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(config =>
        config.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Authentication Sample WebApi");
app.MapWeatherForecastEndpoints();
app.MapApiEndpoints();

app.Run();
