using RandomStore.DataAccess.Data;
using RandomStore.DataAccess.Repository.IRepository;
using RandomStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj) {
            
            var objcFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objcFromDb != null) {
                objcFromDb.Title = obj.Title;
                objcFromDb.ISBN = obj.ISBN;
                objcFromDb.Price = obj.Price;
                objcFromDb.Price50 = obj.Price;
                objcFromDb.ListPrice = obj.ListPrice;
                objcFromDb.Price100 = obj.Price100;
                objcFromDb.Description = obj.Description;
                objcFromDb.CategoryId = obj.CategoryId;
                objcFromDb.Author = obj.Author;
                if (obj.ImageUrl != null) {
                    objcFromDb.ImageUrl = obj.ImageUrl;
                }
            }

        }

        

    }
}
