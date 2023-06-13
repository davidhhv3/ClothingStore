using ClothingStore.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddDbContexts(builder.Configuration);
builder.Services.AddServices();

//builder.Services.AddMvc().
//builder.Services.AddFluentValidationAutoValidation();
//builder.Services
//    .AddFluentValidationAutoValidation()
//    .AddValidatorsFromAssembly(AppDomain.CurrentDomain.GetAssemblies()[0]);

builder.Services.AddMvc(options =>
{
}).AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});




//options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());






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
