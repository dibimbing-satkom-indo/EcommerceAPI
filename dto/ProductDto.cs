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