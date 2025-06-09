namespace MoneyManager.Application.DTOs;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
}