using Carter;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add Services to the container
builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services.AddBasketModule()
                .AddCatalogModule(builder.Configuration)
                .AddOrderingModule();


var app = builder.Build();

//Configuring Http pipeline
app.UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app.MapCarter();

app.Run();
