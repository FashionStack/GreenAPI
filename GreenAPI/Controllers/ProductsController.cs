using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenAPI.Context;
using GreenAPI.Models;
using System.Net;
using GreenAPI.Models.ViewModels;

namespace GreenAPI.Controllers
{
    /// <summary>
    /// Métodos relacionados à manipulação de produtos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly GreenStockContext _context;

        public ProductsController(GreenStockContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Buscar todos os produtos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.Include(t => t.Category).ToListAsync();
        }

        /// <summary>
        /// Buscar uma produto baseado em seu ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            Product product = await _context.Product.Include(t => t.Category).FirstOrDefaultAsync(i => i.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Editar um produto baseado em seu ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            //Valida se a categoria informada existe
            Category category = await _context.Category.FindAsync(product.CategoryId);

            if (category == null || category.Status == false)
            {
                return NotFound(new Error
                {
                    Code = HttpStatusCode.NotFound,
                    Message = "A categoria informada não existe."
                }); ;
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Incluir um novo produto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //Valida se a categoria informada existe
            Category category = await _context.Category.FindAsync(product.CategoryId);

            if (category == null || category.Status == false)
            {
                return NotFound(new Error
                {
                    Code = HttpStatusCode.NotFound,
                    Message = "A categoria informada não existe."
                }); ;
            }

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        /// <summary>
        /// Deletar um produto baseado em seu ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            Product product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Obter informações do dashboard de produtos.
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardViewModel>> GetProductDashboard()
        {
            List<Product> products = await _context.Product.Include(t => t.Category).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            try
            {
                var dashboard = new DashboardViewModel()
                {
                    ProductsCount = products.Count,
                    StockItemsCount = products.Sum(p => p.Amount),
                    SustainableItemsCount = products.Where(p => p.Sustainable).Sum(p => p.Amount),
                    NonSustainableItemsCount = products.Where(p => !p.Sustainable).Sum(p => p.Amount),
                };

                var categories = new List<DashboardCategoriesCountViewModel>();
                var groupby = products.GroupBy(i => new { i.CategoryId, i.Category.Name }).Select(i => new { CategoryId = i.Key.CategoryId, Name = i.Key.Name }).ToList();

                foreach (var item in groupby)
                {
                    DashboardCategoriesCountViewModel category = new DashboardCategoriesCountViewModel()
                    {
                        Category = item.Name,
                        SustainableItemsCount = products.Where(i => i.CategoryId == item.CategoryId && i.Sustainable == true).Sum(i => i.Amount),
                        NonSustainableItemsCount = products.Where(i => i.CategoryId == item.CategoryId && i.Sustainable == false).Sum(i => i.Amount)
                    };

                    categories.Add(category);
                }

                dashboard.Categories = categories;

                return dashboard;
            }
            catch 
            {
                return UnprocessableEntity();
            }
        }

        private bool ProductExists(long id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
