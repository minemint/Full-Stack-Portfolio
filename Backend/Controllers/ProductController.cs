using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetStockAPI.Controllers;

// Multiple Roles
//role only Admin , Manager
[Authorize]
[ApiController]
[Route("api/[controller]")]
[EnableCors("MultipleOrigins")]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    //env
    private readonly IWebHostEnvironment _env;

    // ฟังก์ชันสร้าง Constructor รับค่า ApplicationDbContext
    public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        //env
        _env = env;
    }

    // GET /api/Product
    [HttpGet]
    public ActionResult<Product> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 100,
        [FromQuery] string? searchQuery = null,
        [FromQuery] int? selectedCategory = null
    )
    {
        int skip = (page - 1) * limit;


        var query = _context.products
        .Join(
            _context.categories,
            p => p.categoryid,
            c => c.categoryid,
            (p, c) => new
            {
                p.productid,
                p.productname,
                p.unitprice,
                p.unitinstock,
                p.productpicture,
                p.categoryid,
                p.createddate,
                p.modifieddate,
                c.categoryname
            }
        );

        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(p => EF.Functions.ILike(p.productname!, $"%{searchQuery}%"));
        }

        if (selectedCategory.HasValue)
        {
            query = query.Where(p => p.categoryid == selectedCategory.Value);
        }

        var totalRecords = query.Count();

        var products = query
        .OrderByDescending(p => p.productid)
        .Skip(skip)
        .Take(limit)
        .ToList();

        return Ok(
            new
            {
                Total = totalRecords,
                Products = products
            }
        );
    }

    // GET /api/Product/1
    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id)
    {

        var product = _context.products
        .Join(
            _context.categories,
            p => p.categoryid,
            c => c.categoryid,
            (p, c) => new
            {
                p.productid,
                p.productname,
                p.unitprice,
                p.unitinstock,
                p.productpicture,
                p.categoryid,
                p.createddate,
                p.modifieddate,
                c.categoryname
            }
        )
        .FirstOrDefault(p => p.productid == id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    // POST: /api/Product
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromForm] Product product, IFormFile? image)
    {
        _context.products.Add(product);

        if (image != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            product.productpicture = fileName;

        }
        else
        {
            product.productpicture = "noimg.jpg";
        }

        _context.SaveChanges();

        return Ok(product);
    }

    // PUT: /api/Product/1
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, [FromForm] Product product, IFormFile? image)
    {
        var existingProduct = _context.products.FirstOrDefault(p => p.productid == id);

        if (existingProduct == null)
        {
            return NotFound();
        }

        existingProduct.productname = product.productname;
        existingProduct.unitprice = product.unitprice;
        existingProduct.unitinstock = product.unitinstock;
        existingProduct.categoryid = product.categoryid;
        existingProduct.modifieddate = product.modifieddate;

        if (image != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            if (existingProduct.productpicture != "noimg.jpg")
            {
                System.IO.File.Delete(Path.Combine(uploadFolder, existingProduct.productpicture!));
            }

            existingProduct.productpicture = fileName;

        }

        _context.SaveChanges();

        return Ok(existingProduct);
    }

    // DELETE /api/Product/1
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public ActionResult<Product> DeleteProduct(int id)
    {
        var product = _context.products.Find(id);

        if (product == null)
        {
            return NotFound();
        }

        if (product.productpicture != "noimg.jpg")
        {
            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

            System.IO.File.Delete(Path.Combine(uploadFolder, product.productpicture!));
        }
        _context.products.Remove(product);

        _context.SaveChanges();

        return Ok(product);
    }

}
