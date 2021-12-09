using basketsvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddActors(
    options => {
        options.Actors.RegisterActor<BasketActor>();
    }
);

var app = builder.Build();
app.UseRouting();
app.UseEndpoints( 
    endpoints => {
        endpoints.MapActorsHandlers();
    } 
);

//app.MapGet("/", () => "BasketActor started!");

app.Run();
