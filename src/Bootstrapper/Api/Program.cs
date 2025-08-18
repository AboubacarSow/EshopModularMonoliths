

var builder = WebApplication.CreateBuilder(args);

//Configuration of serilog
builder.Host.UseSerilog((context, config)=>{
    config.ReadFrom.Configuration(context.Configuration);
});

//Add Services to the container
builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services.AddBasketModule()
                .AddCatalogModule(builder.Configuration)
                .AddOrderingModule();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configuring Http pipeline
app.UseBasketModule()
    .UseCatalogModule()
    .UseOrderingModule();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();

app.Run();
