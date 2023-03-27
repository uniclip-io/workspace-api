using WorkspaceApi.Services;

foreach (var line in File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), ".env")))
{
    var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
    Environment.SetEnvironmentVariable(parts[0], string.Join("=", parts.Skip(1)));
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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