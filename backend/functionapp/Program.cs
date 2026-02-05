using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFunctionsWorkerDefaults();

var app = builder.Build();

app.ConfigureFunctionsWebApplication();

app.Run();
