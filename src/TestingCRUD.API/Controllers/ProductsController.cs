using FluentValidation;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.InputModels.ProductInputModels;
using TestingCRUD.Application.Queries.ProductQueries;

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

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, null);
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
