using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TestingCRUD.Aplication.Shared;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.Commands.OrderCommands;
using TestingCRUD.Application.Commands.OrderItemCommands;
using TestingCRUD.Application.Commands.ProductCommands;
using TestingCRUD.Application.Handlers.CustomerHandlers;
using TestingCRUD.Application.Handlers.OrderHandlers;
using TestingCRUD.Application.Handlers.OrderItemHandlers;
using TestingCRUD.Application.Handlers.ProductHandlers;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Queries.CustomerQueries;
using TestingCRUD.Application.Queries.OrderQueries;
using TestingCRUD.Application.Queries.ProductQueries;
using TestingCRUD.Application.Validations.CustomerCommandValidation;
using TestingCRUD.Application.Validations.OrderCommandValidation;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.ViewModels.CustomerViewModels;
using TestingCRUD.Application.ViewModels.ProductViewModels;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Infra;
using TestingCRUD.Infra.QueryHandlers.ProductsQueryHandlers;
using TestingCRUD.Infra.Repositories;
using TestingCRUD.UI.Components;

var builder = WebApplication.CreateBuilder(args);

// MudBlazor
builder.Services.AddMudServices();

// DB Context (ajuste a connection string)
var connectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

// MediatR - registre apenas uma vez apontando para um assembly, isso já cobre tudo!
builder.Services.AddMediatR(typeof(ActivateCustomerCommand).Assembly);

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();

// Handlers (exemplo, você pode manter ou escanear por assembly)
builder.Services.AddScoped<IRequestHandler<GetCustomersQuery, IEnumerable<CustomerViewModel>>, GetCustomersQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetCustomerByCpfQuery, CustomerViewModel>, GetCustomerByCpfQueryHandler>();
builder.Services.AddScoped<IRequestHandler<CreateCustomerCommand, Result<CustomerViewModel>>, CreateCustomerCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCustomerCommand, Result<bool>>, UpdateCustomerCommandHandler>();
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

// Repositórios (Essencial para resolver dependências dos Handlers)
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();

// Razor Components
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// Se quiser rodar migrations ou seed, faça aqui:
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();
    InitializeDB.Initialize(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
