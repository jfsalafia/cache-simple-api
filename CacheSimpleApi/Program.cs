using CacheSimpleApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddFusionCache(opt =>
    opt.DefaultEntryOptions = new ZiggyCreatures.Caching.Fusion.FusionCacheEntryOptions()
    {
        Duration = TimeSpan.FromMinutes(1),
        Priority = Microsoft.Extensions.Caching.Memory.CacheItemPriority.Low
    });

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
