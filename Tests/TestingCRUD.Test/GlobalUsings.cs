global using Moq;
global using Xunit;
global using ValidationException = FluentValidation.ValidationException;

global using TestingCRUD.Domain.Enums;
global using TestingCRUD.Domain.Models;
global using TestingCRUD.Domain.Repositories;
global using TestingCRUD.Application.InputModels;
global using TestingCRUD.Application.Handlers.CustomerHandlers;
global using TestingCRUD.Application.Commands.CustomerCommands;
global using TestingCRUD.Application.ViewModels.CustomerViewModels;
global using TestingCRUD.Application.Validations.CustomerCommandValidation;