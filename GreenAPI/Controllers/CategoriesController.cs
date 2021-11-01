﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenAPI.Context;
using GreenAPI.Models;
using Swashbuckle.AspNetCore;
using System.Net;

namespace GreenAPI.Controllers
{
    /// <summary>
    /// Métodos relacionados à manipulação de categorias.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly GreenStockContext _context;

        public CategoriesController(GreenStockContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Buscar todas as categorias disponíveis.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return await _context.Category.Where(x => x.Status == true).ToListAsync();
        }

        /// <summary>
        /// Buscar uma categoria baseado em seu ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        /// <summary>
        /// Incluir uma nova categoria.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            //Seta a categoria como ativa
            category.Status = true;

            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        /// <summary>
        /// Deletar uma categoria.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            category.Status = false;
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}
