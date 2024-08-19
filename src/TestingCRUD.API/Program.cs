using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using TestingCRUD.Infra;
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
using TestingCRUD.Application.Queries.OrderQueries;
using TestingCRUD.Application.Handlers.OrderHandlers;
using TestingCRUD.Application.Commands.OrderCommands;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.Validations.OrderCommandValidation;
using TestingCRUD.Application.Commands.OrderItemCommands;
using TestingCRUD.Application.Handlers.OrderItemHandlers;

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
builder.Services.AddScoped<IRequestHandler<InactivateProductCommand, bool>, InactivateProductCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ActivateProductCommand, bool>, ActivateProductCommandHandler>();
builder.Services.AddScoped<IRequestHandler<IncreaseStockCommand, bool>, IncreaseStockCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DecrementStockCommand, bool>, DecrementStockCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ChangePriceCommand, bool>, ChangePriceCommandHandler>();

builder.Services.AddScoped<IRequestHandler<GetOrdersQuery, IEnumerable<OrderViewModel>>, GetOrdersQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetOrderByIdQuery, OrderViewModel>, GetOrderByIdQueryHandler>();
builder.Services.AddScoped<IRequestHandler<CreateOrderCommand, OrderViewModel>, CreateOrderCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ExchangeStatusAwaitingPaymentCommand, bool>, ExchangeStatusAwaitingPaymentCommandHandler>();
builder.Services.AddScoped<IRequestHandler<ExchangeStatusConcludeCommand, bool>, ExchangeStatusConcludeCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveOrderCommand, bool>, RemoveOrderCommandHandler>();

builder.Services.AddScoped<IRequestHandler<UpdateQuantityItemCommand, bool>, UpdateQuantityItemCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RemoveItemCommand, bool>, RemoveItemCommandHandler>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();

builder.Services.AddScoped<IValidator<OrderInputModel>, CreateOrderCommandValidator>();

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
