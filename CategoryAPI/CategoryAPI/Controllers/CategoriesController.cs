using CategoryAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CategoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MyContext db;
        public CategoriesController(MyContext db) 
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var Categories = await db.Categories.ToListAsync();
            return Ok(Categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int Id)
        {
            var Categories = await db.Categories.SingleOrDefaultAsync(x=>x.Id == Id);
            if(Categories == null)
            {
                return NotFound($"There's no Category in this Id : {Id}");
            }
            return Ok(Categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategories(string Categorie)
        {
            Category c = new() { Name = Categorie };
            await db.Categories.AddAsync(c);
            db.SaveChanges();
            return Ok(c);
        }
        //ydir update lkoulchi
        [HttpPut]
        public async Task<IActionResult> UpdateCategories(Category Cat)
        {
            var c = await db.Categories.SingleOrDefaultAsync(x => x.Id == Cat.Id);
            if( c == null)
            {
                return NotFound($"Category Id {Cat.Id} not exist");
            }
            c.Name = Cat.Name;
            db.SaveChanges();
            return Ok(c);
        }

        //Difference entre Put et Patch
        //w lpatch y9der ydir update l haja whda wla bzaf
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategoriesPatch([FromBody] JsonPatchDocument<Category> Category, [FromRoute] int id)
        {
            var c = await db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category Id {id} not exist");
            }
            Category.ApplyTo(c);
            await db.SaveChangesAsync();
            return Ok(c);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategories (int id)
        {
            var c = await db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category Id {id} not exist");
            }
            db.Categories.Remove(c);
            db.SaveChanges();
            return Ok(c);
        }
        
    }
}
