using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DotnetStockAPI.Controllers;

// Multiple Roles
//role only Admin , Manager
[Authorize]
[ApiController]
[Route("api/[controller]")]
[EnableCors("MultipleOrigins")]
public class CategoryController : ControllerBase
{

    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // CRUD Category

    // GET /api/Category
    [HttpGet]
    public ActionResult<Category> GetCategories()
    {
        var categories = _context.categories.ToList();
        return Ok(categories);
    }

    // GET /api/Category/1
    [HttpGet("{id}")]
    public ActionResult<Category> GetCategory(int id)
    {
        var category = _context.categories.Find(id);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    // POST /api/Category
    [HttpPost]
    public ActionResult<Category> AddCategory([FromBody] Category category)
    {
        _context.categories.Add(category);
        _context.SaveChanges(); // commit

        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(category);
    }

    // PUT /api/Category/1
    [HttpPut("{id}")]
    public ActionResult<Category> UpdateCategory(int id, [FromBody] Category category)
    {
        var cate = _context.categories.Find(id);

        if (cate == null)
        {
            return NotFound();
        }

        cate.categoryname = category.categoryname;
        cate.categorystatus = category.categorystatus;

        _context.SaveChanges();

        return Ok(cate);
    }

    // DELETE /api/Category/1
    [HttpDelete("{id}")]
    public ActionResult<Category> DeleteCategory(int id)
    {
        var cat = _context.categories.Find(id);

        if (cat == null)
        {
            return NotFound();
        }

        // ลบข้อมูล Category
        _context.categories.Remove(cat);
        _context.SaveChanges();

        return Ok(cat);
    }

}