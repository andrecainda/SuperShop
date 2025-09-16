using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        

        private readonly IProductRepository _productrepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;


        public ProductsController(
            IProductRepository productrepository, 
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper
            )
        {
            _productrepository = productrepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Products
        /*
        public IActionResult Index()
        {
            return View(_productrepository.GetAll());
        }
        */

        public IActionResult Index()
        {
             return View(_productrepository.GetAll().OrderBy(p => p.Name));
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productrepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
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
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");

          
                }

                //var product = this.ToProduct(model, path);

                var product = _converterHelper.ToProduct(model, path, true);



                //TODO: Modificar para o user que tiver logado
                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _productrepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productrepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }
            //var model = this.ToProductViewModel(product);
            var model = _converterHelper.ToProductViewModel(product);
            return View(model);
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

                    var path = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                    }

                   // var product = this.ToProduct(model, path);
                    var product = _converterHelper.ToProduct(model, path, false);

                   
                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
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
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productrepository.GetByIdAsync(id.Value);   
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
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

        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}
