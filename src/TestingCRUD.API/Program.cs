using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using TestingCRUD.Infra;
using TestingCRUD.Domain.Models;
using TestingCRUD.Infra.Repositories;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Queries.CustomerQueries;
using TestingCRUD.Aplication.Commands.CustomerCommands;
using TestingCRUD.Aplication.Handlers.CustomerHandlers;
using TestingCRUD.Aplication.ViewModels.CustomerViewModels;
using TestingCRUD.Aplication.Validations.CustomerCommandValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");
builder.Services.AddDbContext<CustomerContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddValidatorsFromAssembly(typeof(UpdateCustomerValidator).Assembly);
//TODO: Criar uma extensão para configurar esses serviços
builder.Services.AddScoped<IRequestHandler<CreateCustomerCommand, Customer>, CreateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCustomerCommand, Customer>, UpdateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveCustomerCommand, Unit>, RemoveCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetCustomerByCpfQuery, CustomerViewModel>, GetCustomerByCpfCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetCustomersQuery, IEnumerable<CustomerViewModel>>, GetCustomersCommandHandler>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();

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
