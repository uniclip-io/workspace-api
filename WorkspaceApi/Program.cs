using WorkspaceApi.Repositories;
using WorkspaceApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(_ => new WorkspaceRepository(builder.Configuration["ConnectionStrings:MongoDb"]!));
builder.Services.AddSingleton(_ => new DeviceRepository(builder.Configuration["ConnectionStrings:MongoDb"]!));
builder.Services.AddScoped<WorkspaceService, WorkspaceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();