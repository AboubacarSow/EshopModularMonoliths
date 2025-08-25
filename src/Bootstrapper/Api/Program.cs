using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);

//Configuration of serilog
builder.Host.UseSerilog((context, config)=>{
    config.ReadFrom.Configuration(context.Configuration);
});

//Add Services to the container
Assembly[] assemblies = [typeof(CatalogModule).Assembly, typeof(BasketModule).Assembly,
typeof(OrderingModule).Assembly];

builder.Services.AddMediatorFromAssemblies(assemblies)
                .AddValidatorsFromAssemblies(assemblies)
                .AddCarterWithAssemblies(assemblies)
                .AddMassTransitWitAssemblies(builder.Configuration,assemblies);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration); 
builder.Services.AddAuthorization(); 


builder.Services.AddBasketModule(builder.Configuration)
                .AddCatalogModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration=builder.Configuration.GetConnectionString("Redis");
});

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

app.UseAuthentication(); 
app.UseAuthorization(); 

app.Run();
