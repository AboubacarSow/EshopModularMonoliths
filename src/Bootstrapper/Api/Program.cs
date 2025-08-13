var builder = WebApplication.CreateBuilder(args);

//Add Services to the container
builder.Services.AddBasketModule()
                .AddCatalogModule(builder.Configuration)
                .AddOrderingModule();


var app = builder.Build();

//Configuring Http pipeline
app.UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();


app.Run();
