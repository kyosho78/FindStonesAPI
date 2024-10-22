using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindStonesAPI.Models;

namespace FindStonesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemHistoriesController : ControllerBase
    {
        private readonly FindStoneDBContext _context;

        public ItemHistoriesController(FindStoneDBContext context)
        {
            _context = context;
        }

        // GET: api/ItemHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemHistory>>> GetItemHistories()
        {
          if (_context.ItemHistories == null)
          {
              return NotFound();
          }
            return await _context.ItemHistories.ToListAsync();
        }

        // GET: api/ItemHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemHistory>> GetItemHistory(int id)
        {
          if (_context.ItemHistories == null)
          {
              return NotFound();
          }
            var itemHistory = await _context.ItemHistories.FindAsync(id);

            if (itemHistory == null)
            {
                return NotFound();
            }

            return itemHistory;
        }

        // PUT: api/ItemHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemHistory(int id, ItemHistory itemHistory)
        {
            if (id != itemHistory.HistoryId)
            {
                return BadRequest();
            }

            _context.Entry(itemHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemHistoryExists(id))
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

        // POST: api/ItemHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemHistory>> PostItemHistory(ItemHistory itemHistory)
        {
          if (_context.ItemHistories == null)
          {
              return Problem("Entity set 'FindStoneDBContext.ItemHistories'  is null.");
          }
            _context.ItemHistories.Add(itemHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemHistory", new { id = itemHistory.HistoryId }, itemHistory);
        }

        // DELETE: api/ItemHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemHistory(int id)
        {
            if (_context.ItemHistories == null)
            {
                return NotFound();
            }
            var itemHistory = await _context.ItemHistories.FindAsync(id);
            if (itemHistory == null)
            {
                return NotFound();
            }

            _context.ItemHistories.Remove(itemHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemHistoryExists(int id)
        {
            return (_context.ItemHistories?.Any(e => e.HistoryId == id)).GetValueOrDefault();
        }
    }
}
