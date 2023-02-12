using SkillfullAPI.Services;
using SkillfullAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ILightcastAccessTokenService, LightcastAccessTokenService>(client =>
client.BaseAddress = new Uri("https://auth.emsicloud.com/connect/token"));
builder.Services.AddHttpClient<ILightcastSkillsApiService, LightcastSkillsApiService>(client =>
client.BaseAddress = new Uri("https://emsiservices.com/skills/versions/latest/"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
