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
    public class UserFoundItemsController : ControllerBase
    {
        private readonly FindStoneDBContext _context;

        public UserFoundItemsController(FindStoneDBContext context)
        {
            _context = context;
        }

        // GET: api/UserFoundItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFoundItem>>> GetUserFoundItems()
        {
          if (_context.UserFoundItems == null)
          {
              return NotFound();
          }
            return await _context.UserFoundItems.ToListAsync();
        }

        // GET: api/UserFoundItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFoundItem>> GetUserFoundItem(int id)
        {
          if (_context.UserFoundItems == null)
          {
              return NotFound();
          }
            var userFoundItem = await _context.UserFoundItems.FindAsync(id);

            if (userFoundItem == null)
            {
                return NotFound();
            }

            return userFoundItem;
        }

        // PUT: api/UserFoundItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFoundItem(int id, UserFoundItem userFoundItem)
        {
            if (id != userFoundItem.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userFoundItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFoundItemExists(id))
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

        // POST: api/UserFoundItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserFoundItem>> PostUserFoundItem(UserFoundItem userFoundItem)
        {
          if (_context.UserFoundItems == null)
          {
              return Problem("Entity set 'FindStoneDBContext.UserFoundItems'  is null.");
          }
            _context.UserFoundItems.Add(userFoundItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserFoundItemExists(userFoundItem.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserFoundItem", new { id = userFoundItem.UserId }, userFoundItem);
        }

        // DELETE: api/UserFoundItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFoundItem(int id)
        {
            if (_context.UserFoundItems == null)
            {
                return NotFound();
            }
            var userFoundItem = await _context.UserFoundItems.FindAsync(id);
            if (userFoundItem == null)
            {
                return NotFound();
            }

            _context.UserFoundItems.Remove(userFoundItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserFoundItemExists(int id)
        {
            return (_context.UserFoundItems?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
