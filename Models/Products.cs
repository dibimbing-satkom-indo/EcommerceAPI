using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("products")]
public class Products
{
    [Key]
    public int ProductID { get; set; }

    [Required]
    [StringLength(100)]

    public string ProductName { get; set; } = string.Empty;

    public int? CategoryID { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    public bool IsActive { get; set; }

    //relation property
    [ForeignKey("CategoryID")]
    public virtual Category? Category { get; set; }

}