using Api.Config;
using Api.Service;
using Api.Services;
using StackExchange.Redis;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") // frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Redis
var redisConfig = builder.Configuration.GetValue<string>("Redis:Configuration");

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var options = ConfigurationOptions.Parse(redisConfig);

    // Recommended for Docker / retry scenarios
    options.AbortOnConnectFail = false;  // Keep retrying if Redis is not ready
    options.ConnectRetry = 5;            // Retry up to 5 times
    options.ConnectTimeout = 5000;       // 5 seconds timeout per attempt

    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddScoped<IRedisService, RedisService>();

// News services
builder.Services.AddScoped<INewsService, NewsService>();


builder.Services.Configure<NewsServiceConfig>(
    builder.Configuration.GetSection("NewsServiceConfig"));

builder.Services.AddTransient<NewsService>();

// ✅ Refit client - must be added BEFORE builder.Build()

var hackerNewsConfig = builder.Configuration.GetSection("HackerNews");
var baseUrl = hackerNewsConfig.GetValue<string>("BaseUrl");
builder.Services
    .AddRefitClient<IHackerNewsApi>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(baseUrl);
    });

var app = builder.Build();

app.UseCors();
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();