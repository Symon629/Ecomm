using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RandomStore.DataAccess.Repository.IRepository;
using RandomStore.Models.Models;
using RandomStore.Models.ViewModels;

namespace RandomStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Product> allProducts = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            // How can we convert an IEnumerable of Cateoires to an IEnumebarable 
            // of selectList Item, we do that using projections.


            //Projectin in efcore 
            // We have something called projectionin ef core
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }) ;

            


            
            return View(allProducts);
        }
        [HttpGet]
        public IActionResult Upsert(int? id) {

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVm = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };


            if (id == null || id == 0)
            {
                return View(productVm);
            }
            else {
                productVm.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVm);
            }

           
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file) {
         
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) {
                    // Guid is for everytime a new image is uploaded a new id is created 
                    // and we name that file with that guid
                    // 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl)) {
                        // Delete the old image
                        // As we have a forwared slash in our ImageUrl field in the database and wewill have to trim that. 
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) {
                            System.IO.File.Delete(oldImagePath);
                                }

               
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName; 
                }
                // if the id is 0 then its an add if not its an update. 

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else {
                    _unitOfWork.Product.Update(productVM.Product);
                }
             
                _unitOfWork.Save();
                TempData["success"] = "Product Created successfully";
                return RedirectToAction("Index");
            }
            else {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u =>  new SelectListItem{
                    Text = u.Name,
                    Value = u.Id.ToString()});
                return View(productVM);
            }
            
        }

        [HttpGet]
        public IActionResult Edit(int? id) {
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product obj) {
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int? id) {
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product obj) {
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        
    }
}
