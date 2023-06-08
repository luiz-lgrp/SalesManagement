using MediatR;

using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.ViewModels.ProductViewModels;

namespace TestingCRUD.Application.Commands.ProductCommands;
public class CreateProductCommand : IRequest<ProductViewModel>
{
    public ProductInputModel ProductModel { get; set; }

    public CreateProductCommand(ProductInputModel productModel) => ProductModel = productModel;
}
