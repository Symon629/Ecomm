using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RandoStore_Temp.Data;
using RandoStore_Temp.Models;

namespace RandoStore_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public void OnGet(int? id)
        {
            Category = _db.Categories.FirstOrDefault(x => x.Id == id);

        }
        public IActionResult OnPost()
        {
            _db.Categories.Update(Category);
            _db.SaveChanges();
            return RedirectToPage("Index");

        }
    }
}
