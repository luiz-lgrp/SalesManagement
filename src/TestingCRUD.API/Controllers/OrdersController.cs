using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Queries.OrderQueries;
using TestingCRUD.Application.Commands.OrderCommands;

namespace TestingCRUD.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var ordersVM = await _mediator.Send(new GetOrdersQuery());

        return Ok(ordersVM);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var orderVM = await _mediator.Send(new GetOrderByIdQuery(id));

        if (orderVM is null)
            return NotFound("O cliente não foi encontrado.");

        return Ok(orderVM);
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderInputModel orderInput)
    {
        try
        {
            var order = await _mediator.Send(new CreateOrderCommand(orderInput));

            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage
            });

            return BadRequest(new { message = "Ocorreram erros na validação", errors });
        }
    }

    [HttpPut("ExchangeAwaitingPayment")]
    public async Task<IActionResult> ExchangeStatusAwaitingPayment([FromQuery] Guid id)
    {
        var UpdatedStatus = await _mediator.Send(new ExchangeStatusAwaitingPaymentCommand(id));

        if (UpdatedStatus is false)
            return NotFound("Pedido não encontrado");

        return Ok(UpdatedStatus);
    }

    [HttpPut("ExchangeStatusConclude")]
    public async Task<IActionResult> ExchangeStatusConclude([FromQuery] Guid id)
    {
        var UpdatedStatus = await _mediator.Send(new ExchangeStatusConcludeCommand(id));

        if (UpdatedStatus is false)
            return NotFound("Pedido não encontrado");

        return Ok(UpdatedStatus);
    }

    [HttpDelete("RemoveOrder")]
    public async Task<IActionResult> RemoveOrder([FromQuery] Guid id)
    {
        var orderDeleted = await _mediator.Send(new RemoveOrderCommand(id));

        if (orderDeleted is false)
            return NotFound("Pedido não encontrado");

        return Ok(orderDeleted);
    }
}
