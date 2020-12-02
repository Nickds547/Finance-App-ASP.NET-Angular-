using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionObjectsController : ControllerBase
    {
        private readonly TransactionObjectContext _context;

        public TransactionObjectsController(TransactionObjectContext context)
        {
            _context = context;
        }

        // GET: api/TransactionObjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionObject>>> GetTransactionObjects()
        {
            return await _context.TransactionObjects.ToListAsync();
        }

        // GET: api/TransactionObjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionObject>> GetTransactionObject(long id)
        {
            var transactionObject = await _context.TransactionObjects.FindAsync(id);

            if (transactionObject == null)
            {
                return NotFound();
            }

            return transactionObject;
        }

        // PUT: api/TransactionObjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionObject(long id, TransactionObject transactionObject)
        {
            if (id != transactionObject.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionObject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionObjectExists(id))
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

        // POST: api/TransactionObjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionObject>> PostTransactionObject(TransactionObject transactionObject)
        {
            _context.TransactionObjects.Add(transactionObject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionObject", new { id = transactionObject.Id }, transactionObject);
        }

        // DELETE: api/TransactionObjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionObject(long id)
        {
            var transactionObject = await _context.TransactionObjects.FindAsync(id);
            if (transactionObject == null)
            {
                return NotFound();
            }

            _context.TransactionObjects.Remove(transactionObject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionObjectExists(long id)
        {
            return _context.TransactionObjects.Any(e => e.Id == id);
        }
    }
}
