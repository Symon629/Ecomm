using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RandoStore_Temp.Data;
using RandoStore_Temp.Models;

namespace RandoStore_Temp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _db;
        public List<Category> Categories { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public void OnGet()
        {
            Categories = _db.Categories.ToList();



        }
    }
}