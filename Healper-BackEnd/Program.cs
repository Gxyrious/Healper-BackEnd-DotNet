using HealperModels;
using HealperService;
using HealperService.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddDbContext<ModelContext>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IScaleService, ScaleServiceImpl>();
builder.Services.AddScoped<IHistoryService, HistoryServiceImpl>();
builder.Services.AddScoped<IConsultService, ConsultServiceImpl>();

builder.Services.AddCors(cors => cors.AddPolicy("anys", p => p.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseWebSockets();

app.UseAuthorization();

app.MapControllers();

app.Run();
