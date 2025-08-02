using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entity;
using SuperShop.Helpers;


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
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                //TODO: Modificar para o user que tiver logado
                product.User = await _userHelper.GetUserByEmailAsync("andretchipalavela@gmail.com");
                await _productrepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,imageUrl,LastPurchase,LastSale,IsAvailable,Stock")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //TODO: Modificar para o user que tiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("andretchipalavela@gmail.com");
                    await _productrepository.UpdateAsync(product);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productrepository.ExistAsync(product.Id))
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
            return View(product);
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
