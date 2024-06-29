using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetStockAPI.Controllers;

// Multiple Roles
// [Authorize(Roles = UserRolesModel.Admin + "," + UserRolesModel.Manager)]
[Authorize]
[ApiController]
[Route("api/[controller]")]
[EnableCors("MultipleOrigins")]
public class ProductController: ControllerBase
{
    // สร้าง Object ของ ApplicationDbContext
    private readonly ApplicationDbContext _context;

    // IWebHostEnvironment คืออะไร
    // ContentRootPath: เส้นทางไปยังโฟลเดอร์รากของเว็บแอปพลิเคชัน
    // WebRootPath: เส้นทางไปยังโฟลเดอร์ wwwroot ของเว็บแอปพลิเคชัน
    private readonly IWebHostEnvironment _env;

    // ฟังก์ชันสร้าง Constructor รับค่า ApplicationDbContext
    public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // ฟังก์ชันสำหรับการดึงข้อมูลสินค้าทั้งหมด
    // GET /api/Product
    [HttpGet]
    public ActionResult<product> GetProducts(
        [FromQuery] int page=1, 
        [FromQuery] int limit=100, 
        [FromQuery] string? searchQuery=null,
        [FromQuery] int? selectedCategory = null
    )
    {
        // skip คือ การข้ามข้อมูล
        int skip = (page - 1) * limit;

        // LINQ สำหรับการดึงข้อมูลจากตาราง Products ทั้งหมด
        // var products = _context.products.ToList();

        // แบบอ่านที่มีเงื่อนไข
        // select * from products where unitinstock >= 10
        // var products = _context.products.Where(p => p.unitinstock >= 10).ToList();

        // แบบเชื่อมกับตารางอื่น products เชื่อมกับ categories
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

        // ถ้ามีการค้นหา
        if(!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(p => EF.Functions.ILike(p.productname!, $"%{searchQuery}%"));
        }

        // ถ้ามีการค้นหาตาม Category
        if(selectedCategory.HasValue)
        {
            query = query.Where(p => p.categoryid == selectedCategory.Value);
        }

        // นับจำนวนข้อมูลทั้งหมด
        var totalRecords = query.Count();

        var products = query
        .OrderByDescending(p => p.productid)
        .Skip(skip)
        .Take(limit)
        .ToList();

        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(
            new {
                Total = totalRecords,
                Products = products
            }
        );
    }

    // ฟังก์ชันสำหรับการดึงข้อมูลสินค้าตาม id
    // GET /api/Product/1
    [HttpGet("{id}")]
    public ActionResult<product> GetProduct(int id)
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

        // ถ้าไม่พบข้อมูล
        if (product == null)
        {
            return NotFound();
        }

        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(product);
    }

    // ฟังก์ชันสำหรับการเพิ่มข้อมูลสินค้า
    // POST: /api/Product
    [HttpPost]
    public async Task<ActionResult<product>> CreateProduct([FromForm] product product, IFormFile? image)
    {
        // เพิ่มข้อมูลลงในตาราง Products
        _context.products.Add(product);

        // ถ้ามีการอัพโหลดไฟล์
        if(image != null)
        {
            // กำหนดชื่อไฟล์รูปภาพใหม่
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            // กำหนดเส้นทางไปยังโฟลเดอร์ uploads
            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

            // ตรวจสอบว่าโฟลเดอร์ uploads มีหรือไม่
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // บันทึกชื่อไฟล์รูปภาพลงในฐานข้อมูล
            product.productpicture = fileName;

        } else {
            product.productpicture = "noimg.jpg";
        }

        _context.SaveChanges();

        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(product);
    }

    // ฟังก์ชันสำหรับการแก้ไขข้อมูลสินค้า
    // PUT: /api/Product/1
    [HttpPut("{id}")]
    public async Task<ActionResult<product>> UpdateProduct(int id, [FromForm] product product, IFormFile? image)
    {
        // ดึงข้อมูลสินค้าตาม id
        var existingProduct = _context.products.FirstOrDefault(p => p.productid == id);

        // ถ้าไม่พบข้อมูล
        if (existingProduct == null)
        {
            return NotFound();
        }

        // แก้ไขข้อมูล
        existingProduct.productname = product.productname;
        existingProduct.unitprice = product.unitprice;
        existingProduct.unitinstock = product.unitinstock;
        existingProduct.categoryid = product.categoryid;
        existingProduct.modifieddate = product.modifieddate;

        // ตรวจสอบว่ามีการอัพโหลดไฟล์รูปภาพหรือไม่
        if(image != null)
        {
            // กำหนดชื่อไฟล์รูปภาพใหม่
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            // กำหนดเส้นทางไปยังโฟลเดอร์ uploads
            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

            // ตรวจสอบว่าโฟลเดอร์ uploads มีหรือไม่
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // ลบไฟล์รูปภาพเดิม ถ้ามีการอัพโหลดรูปภาพใหม่ และรูปภาพเดิมไม่ใช่ noimg.jpg
            if(existingProduct.productpicture != "noimg.jpg"){
                System.IO.File.Delete(Path.Combine(uploadFolder, existingProduct.productpicture!));
            }

            // บันทึกชื่อไฟล์รูปภาพลงในฐานข้อมูล
            existingProduct.productpicture = fileName;

        }

        // บันทึกการแก้ไขข้อมูล
        _context.SaveChanges();

        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(existingProduct);
    }

    // ฟังก์ชันสำหรับการลบข้อมูลสินค้า
    // DELETE /api/Product/1
    [HttpDelete("{id}")]
    public ActionResult<product> DeleteProduct(int id)
    {
        // ค้นหาข้อมูลจากตาราง Products ตาม ID
        var product = _context.products.Find(id);

        // ถ้าไม่พบข้อมูล
        if (product == null)
        {
            return NotFound();
        }

        // ตรวจสอบว่ามีไฟล์รูปภาพหรือไม่
        if(product.productpicture != "noimg.jpg"){
            // string uploadFolder = Path.Combine(_env.ContentRootPath, "uploads");
            string uploadFolder = Path.Combine(_env.WebRootPath, "uploads");

            // ลบไฟล์รูปภาพ
            System.IO.File.Delete(Path.Combine(uploadFolder, product.productpicture!));
        }

        // ลบข้อมูล
        _context.products.Remove(product); // delete from products where id = 1

        // บันทึกการลบข้อมูล
        _context.SaveChanges(); // commit

        // ส่งข้อมูลกลับไปให้ Client เป็น JSON
        return Ok(product);
    }

}
