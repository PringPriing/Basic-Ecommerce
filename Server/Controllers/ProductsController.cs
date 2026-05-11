using Ecommerce.Server.Services;
using Ecommerce.Shared.DTOs.Common;
using Ecommerce.Shared.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IBlobStorageService _blobService;

    public ProductsController(IProductService productService, IBlobStorageService blobService)
    {
        _productService = productService;
        _blobService = blobService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductSummaryDto>>> Search([FromQuery] ProductSearchRequest request) =>
        Ok(await _productService.SearchAsync(request));

    [HttpGet("suggest")]
    public async Task<ActionResult<IList<ProductSummaryDto>>> Suggest([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            return Ok(Array.Empty<ProductSummaryDto>());
        return Ok(await _productService.SuggestAsync(q));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductRequest request)
    {
        var created = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductRequest request)
    {
        var updated = await _productService.UpdateAsync(id, request);
        return updated == null ? NotFound() : Ok(updated);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/image")]
    public async Task<ActionResult<ProductDto>> UploadImage(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided." });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { message = "Only image files are allowed." });

        await using var stream = file.OpenReadStream();
        var (url, blobName) = await _blobService.UploadAsync(stream, file.FileName, file.ContentType);

        var product = await _productService.UpdateImageAsync(id, url, blobName);
        return product == null ? NotFound() : Ok(product);
    }
}
