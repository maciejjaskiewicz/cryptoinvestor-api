using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Transaction;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CryptoInvestor.Api.Controllers
{
    [Authorize]
    public class TransactionsController : ApiControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ICommandDispatcher commandDispatcher,
            ITransactionService transactionService) : base(commandDispatcher)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transactions = await _transactionService.BrowseAsync(UserId);

            if (transactions == null)
            {
                return NotFound();
            }

            return Json(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var transaction = await _transactionService.GetAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Json(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateTransaction command)
        {
            command.Id = Guid.NewGuid();
            await DispatchAsync(command);
            return Created($"transactions/{command.Id}", null);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateTransaction command)
        {
            await DispatchAsync(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _transactionService.DeleteAsync(id);
            return Ok();
        }
    }
}