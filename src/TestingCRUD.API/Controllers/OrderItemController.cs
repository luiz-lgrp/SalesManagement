using MediatR;
using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Commands.OrderItemCommands;
using TestingCRUD.Domain.Models;
using TestingCRUD.Application.Queries.OrderQueries;
using TestingCRUD.Application.DTOs;

namespace TestingCRUD.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderItemController(IMediator mediator) => _mediator = mediator;


    [HttpPut("UpdateQuantityItem")]
    public async Task<IActionResult> UpdateQuantityItem([FromQuery] Guid orderId, [FromBody] ChangeQuantityItemDTO model)
    {
        var QuantityUpdated = await _mediator.Send(new UpdateQuantityItemCommand(orderId, model));

        if (QuantityUpdated is false)
            return NotFound("O pedido ou item não foi encontrado");

        return Ok(QuantityUpdated);
    }

    [HttpDelete("RemoveItem")]
    public async Task<IActionResult> RemoveItem([FromQuery] Guid orderId, [FromQuery] Guid productId)
    {
        var itemDeleted = await _mediator.Send(new RemoveItemCommand(orderId, productId));

        if (itemDeleted is false)
            return NotFound("O pedido ou item não foi encontrado");

        return Ok(itemDeleted);
    }
}
