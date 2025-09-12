using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    public int CategoryID { get; set; }

    [Required]
    [StringLength(100)]
    [Column("CategoriName")]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    //navvigation property - One to many relationship
    public virtual ICollection<Products> Products { get; set; } = new List<Products>();

}