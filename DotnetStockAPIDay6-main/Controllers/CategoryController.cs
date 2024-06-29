using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DotnetStockAPI.Controllers;

// Multiple Roles
// [Authorize(Roles = UserRolesModel.Admin + "," + UserRolesModel.Manager)]
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
    // ฟังก์ชันสำหรับการดึงข้อมูล Category ทั้งหมด
    // GET /api/Category
    [HttpGet]
    public ActionResult<category> GetCategories()
    {
        var categories = _context.categories.ToList(); 

        return Ok(categories);
    }

    // ฟังก์ชันสำหรับการดึงข้อมูล Category ตาม ID
    [HttpGet("{id}")]
    public ActionResult<category> GetCategory(int id)
    {
        // LINQ สำหรับการดึงข้อมูลจากตาราง Categories ตาม ID
        var category = _context.categories.Find(id); 

        // ถ้าไม่พบข้อมูล
        if(category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    // ฟังก์ชันสำหรับการเพิ่มข้อมูล Category
    [HttpPost]
    public ActionResult<category> AddCategory([FromBody] category category)
    {
       // เพิ่มข้อมูลลงในตาราง Categories
        _context.categories.Add(category); // insert into category values (...)
        _context.SaveChanges(); // commit

        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(category);
    }

    // ฟังก์ชันสำหรับการแก้ไขข้อมูล Category
    [HttpPut("{id}")]
    public ActionResult<category> UpdateCategory(int id, [FromBody] category category)
    {
        // ค้นหาข้อมูล Category ตาม ID
        var cat = _context.categories.Find(id);
        if(cat == null)
        {
            return NotFound();
        }

        // แก้ไขข้อมูล Category
        cat.categoryname = category.categoryname; 
        cat.categorystatus = category.categorystatus; 

        // commit
        _context.SaveChanges();

        // ส่งข้อมูลกลับไปให้  เป็น JSON
        return Ok(cat);
    }

    // ฟังก์ชันสำหรับการลบข้อมูล Category
    [HttpDelete("{id}")]
    public ActionResult<category> DeleteCategory(int id)
    {
        var cat = _context.categories.Find(id); // select * from category where id = 1

        if(cat == null)
        {
            return NotFound();
        }

        _context.categories.Remove(cat); 
        _context.SaveChanges(); 
        return Ok(cat);
    }

}