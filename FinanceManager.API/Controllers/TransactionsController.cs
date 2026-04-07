using FinanceManager.Application.DTOs.Transactions;
using FinanceManager.Application.Services;
using FinanceManager.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TransactionsController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionsController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TransactionResponse>>> GetAll(
        [FromQuery] Guid? userId,
        [FromQuery] int? year,
        [FromQuery] int? month,
        [FromQuery] TransactionType? type,
        CancellationToken cancellationToken)
    {
        var transactions = await _transactionService.GetAllAsync(userId, year, month, type, cancellationToken);
        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await _transactionService.GetByIdAsync(id, cancellationToken);
        return Ok(transaction);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionResponse>> Create(
        [FromBody] CreateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TransactionResponse>> Update(
        Guid id,
        [FromBody] UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await _transactionService.UpdateAsync(id, request, cancellationToken);
        return Ok(transaction);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _transactionService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
