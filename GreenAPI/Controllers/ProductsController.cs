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
using Swashbuckle.AspNetCore.Annotations;

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
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var products = await _context.Product.Include(t => t.Category).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        /// <summary>
        /// Buscar uma produto baseado em seu ID.
        /// </summary>
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _context.Product.Include(t => t.Category).FirstOrDefaultAsync(i => i.ProductId == id);

            if (product == null)
            {
                return NoContent();
            }

            return product;
        }

        /// <summary>
        /// Editar um produto baseado em seu ID.
        /// </summary>
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
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
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
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
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
        /// Adicionar ou remover o estoque de um produto baseado em seu ID.
        /// </summary>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        [HttpPut("amount")]
        public async Task<IActionResult> PutProductStockAmount(ProductAmount value)
        {
            //Valida se a o produto informado existe
            Product product = await _context.Product.FindAsync(value.ProductId);

            if (product != null)
            {
                //Valida se o valor é negativo e se há a quantidade necessária em estoque para retirada
                if (value.Amount < 0 && value.Amount < product.Amount)
                {
                    return BadRequest(new Error
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "A quantidade informada para retirada é maior que o estoque disponível."
                    });
                }
                else
                {
                    product.Amount += value.Amount;
                }

                _context.Entry(product).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound(new Error
                {
                    Code = HttpStatusCode.NotFound,
                    Message = "O produto informado não existe."
                });
            }

            return Ok();
        }

        /// <summary>
        /// Obter informações do dashboard de produtos.
        /// </summary>
        [ProducesResponseType(typeof(Dashboard), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [HttpGet("dashboard")]
        public async Task<ActionResult<Dashboard>> GetProductDashboard()

        {
            List<Product> products = await _context.Product.Include(t => t.Category).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            try
            {
                var dashboard = new Dashboard()
                {
                    ProductsCount = products.Count,
                    StockItemsCount = products.Sum(p => p.Amount),
                    SustainableItemsCount = products.Where(p => p.Sustainable).Sum(p => p.Amount),
                    NonSustainableItemsCount = products.Where(p => !p.Sustainable).Sum(p => p.Amount),
                };

                var categories = new List<DashboardCategoriesCount>();
                var groupby = products.GroupBy(i => new { i.CategoryId, i.Category.Name }).Select(i => new { CategoryId = i.Key.CategoryId, Name = i.Key.Name }).ToList();

                foreach (var item in groupby)
                {
                    DashboardCategoriesCount category = new DashboardCategoriesCount()
                    {
                        Category = item.Name,
                        SustainableItemsCount = products.Where(i => i.CategoryId == item.CategoryId && i.Sustainable == true).Sum(i => i.Amount),
                        NonSustainableItemsCount = products.Where(i => i.CategoryId == item.CategoryId && i.Sustainable == false).Sum(i => i.Amount)
                    };

                    categories.Add(category);
                }

                dashboard.Categories = categories.OrderByDescending(i => i.SustainableItemsCount).ThenBy(i => i.NonSustainableItemsCount).Take(5).ToList(); ;

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
