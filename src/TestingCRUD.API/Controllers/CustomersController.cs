using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using TestingCRUD.Application.Queries.CustomerQueries;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.InputModels.CustomerInputModels;

namespace TestingCRUD.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var customersVM = await _mediator.Send(new GetCustomersQuery());

        return Ok(customersVM);
    }

    [HttpGet("GetByCpf")]
    public async Task<IActionResult> GetByCpf([FromQuery] string cpf)
    {
        var customerVM = await _mediator.Send(new GetCustomerByCpfQuery(cpf));

        if (customerVM is null)
            return NotFound("O cliente não foi encontrado.");

        return Ok(customerVM);
    }
    
    [HttpPost("CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerInputModel model)
    {
        try
        {
            var customer = await _mediator.Send(new CreateCustomerCommand(model));

            return CreatedAtAction(nameof(GetByCpf), new { cpf = customer.Cpf }, null);
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

    [HttpPut("UpdateCustomer")]
    public async Task<IActionResult> UpdateCustomer([FromQuery] string cpf, [FromBody] CustomerInputModel model)
    {
        try
        {
            var updateCustomer = await _mediator.Send(new UpdateCustomerCommand(cpf, model));

            if (updateCustomer is false)
                return NotFound("O cliente não foi encontrado.");

            return NoContent();
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

    [HttpDelete("RemoveCustomer")]
    public async Task<IActionResult> RemoveCustomer([FromQuery] string cpf)
    {
        var customerDeleted = await _mediator.Send(new RemoveCustomerCommand(cpf));

        if (customerDeleted is false) 
            return NotFound("Cliente não encontrado");

        return Ok(customerDeleted);
    }

    [HttpPut("Inactivate")]
    public async Task<IActionResult> Inactivate([FromQuery] string cpf)
    {
        var inactivateCustomer = await _mediator.Send(new InactivateCustomerCommand(cpf));

        return Ok(inactivateCustomer);
    }

    [HttpPut("Activate")]
    public async Task<IActionResult> Activate([FromQuery] string cpf)
    {
        var inactivateCustomer = await _mediator.Send(new ActivateCustomerCommand(cpf));

        return Ok(inactivateCustomer);
    }
}
