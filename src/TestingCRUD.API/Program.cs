using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.ViewModels;
using TestingCRUD.Infra.Repositories;
using TestingCRUD.Aplication.Queries;
using TestingCRUD.Aplication.Commands;
using TestingCRUD.Aplication.Handlers;
using TestingCRUD.Domain.Repositories;

using MediatR;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddValidatorsFromAssembly(typeof(TestingCRUD.Aplication.Validations.UpdateCustomerValidator).Assembly);

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
