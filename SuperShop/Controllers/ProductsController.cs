using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entity;
using SuperShop.Helpers;
using SuperShop.Models;


namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        public enum SortField
        {
            Name,
            Price,
            Stock,
            Availability
        }

        private readonly IProductRepository _productrepository;
        private readonly IUserHelper _userHelper;

        public ProductsController(
            IProductRepository productrepository, IUserHelper userHelper)
        {
           _productrepository = productrepository;
            _userHelper = userHelper;
        }

        // GET: Products
        /*
        public IActionResult Index()
        {
            return View(_productrepository.GetAll());
        }
        */

        public IActionResult Index(SortField sortField = SortField.Name, bool ascending = true)
        {
            Expression<Func<Product, object>> orderBy = sortField switch
            {
                SortField.Name => p => p.Name,
                SortField.Price => p => p.Price,
                SortField.Stock => p => p.Stock,
                SortField.Availability => p => p.IsAvailable,
                _ => p => p.Name
            };

            var products = _productrepository.GetAll(orderBy, ascending).ToList();

            ViewData["SortField"] = sortField;
            ViewData["Ascending"] = ascending;

            return View(products);
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productrepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {

                var path = string.Empty;
                if(model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";


                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot\\images\\products",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create)) 
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    } 

                    path = $"~/images/products/{file}";
                }

                var product = this.ToProduct(model, path);



                //TODO: Modificar para o user que tiver logado
                product.User = await _userHelper.GetUserByEmailAsync("andretchipalavela@gmail.com");
                await _productrepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product
            {
                Id = model.Id,
                ImageUrl = path,//ImageUrl
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productrepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            var model = this.ToProductViewModel(product);
            return View(model);
        }

        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {


            if (ModelState.IsValid)
            {
                try
                {

                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\products",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/products/{file}";

                    }

                    var product = this.ToProduct(model, path);

                    //TODO: Modificar para o user que tiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("andretchipalavela@gmail.com");
                    await _productrepository.UpdateAsync(product);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productrepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productrepository.GetByIdAsync(id.Value);   
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product =await _productrepository.GetByIdAsync(id);
            await _productrepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }

    }
}
