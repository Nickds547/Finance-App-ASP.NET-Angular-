﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;
using server.JWT;
using Microsoft.Net.Http.Headers;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionObjectContext _context;
        private TransactionService _transactionService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public TransactionController(TransactionObjectContext context, IJwtAuthManager jwtAuthManager)
        {
            _context = context;
            _transactionService = new TransactionService(_context);
            _jwtAuthManager = jwtAuthManager;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionObject>>> GetTransactionObjects()
        {
            var accessToken = GetAccessToken();

            int userId = _jwtAuthManager.GetUserIdFromJwtToken(accessToken);

            var transactionList = await _transactionService.GetTransactionsByUserId(userId);
            
            return Ok(transactionList);
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionObject>> GetTransactionObject(long id)
        {
            var transactionObject = await _transactionService.getTransactionById(id);

            if (transactionObject == null)
            {
                return NotFound();
            }

            int userId = _jwtAuthManager.GetUserIdFromJwtToken(GetAccessToken());

            //If the user is not requesting their own transaction
            if (transactionObject.Id != userId)
            {
                return BadRequest();
            }

            return transactionObject;
        }

        // PUT: api/Transaction/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionObject(long id, TransactionObject transactionObject)
        {
            int userId = _jwtAuthManager.GetUserIdFromJwtToken(GetAccessToken());

            if (userId != transactionObject.Id)
                return BadRequest();

            await _transactionService.UpdateTransaction(id, transactionObject);

            return NoContent();
        }

        // POST: api/Transaction
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionObject>> PostTransactionObject(TransactionObject transactionObject)
        {
            var transaction = await _transactionService.AddTransaction(transactionObject);

            return CreatedAtAction("GetTransactionObject", new { id = transactionObject.Id }, transactionObject);
        }

        // DELETE: api/Transaction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionObject(long id)
        {
            int userId = _jwtAuthManager.GetUserIdFromJwtToken(GetAccessToken());

            var transaction = await _transactionService.DeleteTransaction(id, userId);

            if (transaction == null)
                return NotFound();

            return NoContent();
        }

        private string GetAccessToken()
        {
           return Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer", "").Replace(" ", "");
        }

        
    }
}
