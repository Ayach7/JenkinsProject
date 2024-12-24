using CategoryAPI.Model;
using CategoryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CategoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly MyContext db;
        public ItemsController(MyContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await db.Items.ToListAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var items = await db.Items.SingleOrDefaultAsync(x=>x.Id==id);
            if(items == null)
            {
                return NotFound($"{id} This item is not found ");
            }
            return Ok(items);
        }
        [HttpGet("ItemWithCategory/{idcategory}")]
        public async Task<IActionResult> GetItemWithCategory(int idcategory)
        {
            var items = await db.Items.Where(x => x.CategoryId == idcategory).ToListAsync();
            if (items == null)
            {
                return NotFound($"This Category {idcategory} has no items ");
            }
            return Ok(items);
        }
        [HttpPost]
        public async Task<IActionResult> AddItems([FromForm]ModelItem item)
        {
            using var stream = new MemoryStream();
            await item.Image.CopyToAsync(stream);
            var Item = new Item
            {
                Name = item.Name,
                Price = item.Price,
                Notes = item.Notes,
                CategoryId = item.CategoryId,
                Image = stream.ToArray()
            };
            await db.Items.AddAsync(Item);
            await db.SaveChangesAsync();
            return Ok(Item);
        }
        //FromBody bach ndkhlou lmodel
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItems(int id, [FromForm] ModelItem item)
        {
            var i = await db.Items.FindAsync(id);
            if(i == null)
            {
                return NotFound($"This item {id} does not exist");
            }
            var CategoryExist = await db.Categories.AnyAsync(x=>x.Id == item.CategoryId);
            if (CategoryExist == null)
            {
                return NotFound($"This Category {id} does not exist");
            }
            if(item.Image != null)
            {
                using var stream = new MemoryStream();
                await item.Image.CopyToAsync(stream);
                i.Image = stream.ToArray();
            }
            i.Name = item.Name;
            i.Price = item.Price;
            i.Notes = item.Notes;
            i.CategoryId = item.CategoryId;
            db.SaveChanges();
            return Ok(i);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItems(int id)
        {
            var i = await db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (i == null)
            {
                return NotFound($"This item {id} does not exist");
            }
            db.Items.Remove(i);
            db.SaveChanges();
            return Ok(i);
        }

    }
}
