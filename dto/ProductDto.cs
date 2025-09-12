using System.ComponentModel.DataAnnotations;

public class CreateProductDto
{
    [Required]
    [StringLength(150)]
    public string ProductName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = " Price must be greate than 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = " stcok cannot be negative")]
    public int Stock { get; set; }

    [Required]
    public int CtegoryID { get; set; }

}

public class ProductResponseDTO
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int Stock { get; set; }
}