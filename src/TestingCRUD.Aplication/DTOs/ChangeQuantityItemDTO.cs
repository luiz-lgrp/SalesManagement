namespace TestingCRUD.Application.DTOs;

public class ChangeQuantityItemDTO
{
    public Guid ProductId { get; set; }
    public int NewQuantity { get; set; }

    public ChangeQuantityItemDTO(Guid productId, int newQuantity)
    {
        ProductId = productId;
        NewQuantity = newQuantity;
    }
}
