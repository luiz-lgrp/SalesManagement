using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using TestingCRUD.Infra;
using TestingCRUD.Domain.Models;
using TestingCRUD.Infra.Repositories;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Queries.CustomerQueries;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.Handlers.CustomerHandlers;
using TestingCRUD.Application.ViewModels.CustomerViewModels;
using TestingCRUD.Application.Validations.CustomerCommandValidation;
using TestingCRUD.Application.Queries.ProductQueries;
using TestingCRUD.Application.ViewModels.ProductViewModels;
using TestingCRUD.Infra.QueryHandlers.ProductsQueryHandlers;
using TestingCRUD.Application.Commands.ProductCommands;
using TestingCRUD.Application.Handlers.ProductHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddValidatorsFromAssembly(typeof(UpdateCustomerValidator).Assembly);

//TODO: Criar uma extensão para configurar esses serviços
builder.Services.AddScoped<IRequestHandler<GetCustomersQuery, IEnumerable<CustomerViewModel>>, GetCustomersQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetCustomerByCpfQuery, CustomerViewModel>, GetCustomerByCpfQueryHandler>();
builder.Services.AddScoped<IRequestHandler<CreateCustomerCommand, CustomerViewModel>, CreateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCustomerCommand, bool>, UpdateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveCustomerCommand, bool>, RemoveCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<InactivateCustomerCommand, bool>, InactivateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ActivateCustomerCommand, bool>, ActivateCustomerCommandHandler>();

builder.Services.AddScoped<IRequestHandler<GetAllProductsQuery, IEnumerable<ProductViewModel>>, GetAllProductsQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetProductByIdQuery, ProductViewModel>, GetProductByIdQueryHandler>();
builder.Services.AddScoped<IRequestHandler<CreateProductCommand, ProductViewModel>, CreateProductCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveProductCommand, bool>, RemoveProductCommandHandler>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();

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
