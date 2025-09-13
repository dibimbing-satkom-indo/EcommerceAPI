using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAll()
    {
        var products = await _productService.GetAllProduct();
        if (products == null)
        {
            return NotFound(new BaseResponseDto<string>
            {
                Success = false,
                Messages = " product not found",
                Data = null
            });
        }
        return Ok(new BaseResponseDto<IEnumerable<ProductResponseDTO>>
        {
            Success = true,
            Messages = "success get all product",
            Data = products
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        return Ok(new BaseResponseDto<ProductResponseDTO>
        {
            Success = true,
            Messages = "success get product",
            Data = product
        });

    }

    [HttpGet("filter")]
    public async Task<ActionResult<PagedResponse<ProductResponseDTO>>> GetProducts([FromQuery] FilterDto filter)
    {
        var result = await _productService.GetProductsAsync(filter);
        return Ok(result);
    }


    [HttpPost]
    public async Task<ActionResult<ProductResponseDTO>> CreateProduct(CreateProductDto productDto)
    {
        var result = await _productService.CreateAsync(productDto);


        return StatusCode(201, new BaseResponseDto<ProductResponseDTO>
        {
            Success = true,
            Messages = "add product is success",
            Data = result

        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, [FromBody] ProducUpdatetDto dto)
    {
        await _productService.UpdateAsync(id, dto);
        return Ok(new { Message = "product update successfully" });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchProduct(int id, ProducUpdatetDto dto)
    {
        try
        {
            var updated = await _productService.PatchAsync(id, dto);
            if (updated == null) return NotFound(new { message = $"product with ID {id} not found" });
            return Ok(new { Message = "product patch successfully" });
        }
        catch (ArgumentException ex)
        {

            return BadRequest(new { Messages = ex.Message });
        }
    }

    //soft delete
    [HttpDelete("soft/{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var result = await _productService.SoftDeleteAsync(id);
        if (!result) return
            NotFound(new { message = $" product ID {id} not found" });
        return StatusCode(
            200,
            new { message = "product soft deleted {IsActive = false}" });
    }

    //hard delete
    [HttpDelete("hard/{id}")]
    public async Task<IActionResult> HardDeleteAsync(int id)
    {
        var result = await _productService.HarddeleteAsync(id);
        if (!result) return
            NotFound(new { message = $" product ID {id} not found" });
        return StatusCode(
            200,
            new { message = "product permanently deleted" });
    }



}