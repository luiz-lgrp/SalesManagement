using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Queries.ProductQueries;
using TestingCRUD.Application.Commands.ProductCommands;

namespace TestingCRUD.API.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var productVM = await _mediator.Send(new GetAllProductsQuery());

        return Ok(productVM);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var productVM = await _mediator.Send(new GetProductByIdQuery(id));

        return Ok(productVM);
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductInputModel model)
    {
        try
        {
            var product = await _mediator.Send(new CreateProductCommand(model));

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
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

    [HttpDelete("RemoveProduct")]
    public async Task<IActionResult> RemoveProduct([FromQuery] Guid id)
    {
        var productDeleted = await _mediator.Send(new RemoveProductCommand(id));

        if (productDeleted is false)
            return NotFound("Produto não encontrado");

        return Ok(productDeleted);
    }

    [HttpPut("Inactivate")]
    public async Task<IActionResult> Inactivate([FromQuery] Guid id)
    {
        var inactivateProduct = await _mediator.Send(new InactivateProductCommand(id));

        if (inactivateProduct is false)
            return NotFound("Produto não encontrado");

        return Ok(inactivateProduct);
    }

    [HttpPut("Activate")]
    public async Task<IActionResult> Activate([FromQuery] Guid id)
    {
        var activateProduct = await _mediator.Send(new ActivateProductCommand(id));

        if (activateProduct is false)
            return NotFound("Produto não encontrado");

        return Ok(activateProduct);
    }

    [HttpPut("IncreaseStock")]
    public async Task<IActionResult> IncreaseStock([FromQuery] Guid id, [FromBody] int quantity)
    {
        var increaseProduct = await _mediator.Send(new IncreaseStockCommand(id, quantity));

        if (increaseProduct is false)
            return NotFound("Produto não encontrado");
        
        return Ok(increaseProduct);
    }

    [HttpPut("DecrementStock")]
    public async Task<IActionResult> DecrementStock([FromQuery] Guid id, [FromBody] int quantity)
    {
        var decrementProduct = await _mediator.Send(new DecrementStockCommand(id, quantity));

        if (decrementProduct is false)
            return NotFound("Produto não encontrado");

        return Ok(decrementProduct);
    }

    [HttpPut("ChangePrice")]
    public async Task<IActionResult> ChangePrice([FromQuery] Guid id, [FromBody] decimal value)
    {
        var alteredPrice = await _mediator.Send(new ChangePriceCommand(id, value));

        if (alteredPrice is false)
            return NotFound("Produto não encontrado");

        return Ok(alteredPrice);
    }
}
