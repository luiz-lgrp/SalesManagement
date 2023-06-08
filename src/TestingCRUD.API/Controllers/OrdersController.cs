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
    //TODO: Vc colocou cancellation token nas operações, TESTAR
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
    //TODO: Implementar o Remove Order
    //[HttpDelete("{id}")]
    //public IActionResult DeleteOrder(Guid id)
    //{
    //    // Procurar o pedido pelo ID no sistema
    //    var order = _orderRepository.GetOrderById(id);

    //    if (order == null)
    //    {
    //        return NotFound();
    //    }

    //    // Excluir o pedido do sistema
    //    _orderRepository.DeleteOrder(order);

    //    return NoContent();
    //}

}
