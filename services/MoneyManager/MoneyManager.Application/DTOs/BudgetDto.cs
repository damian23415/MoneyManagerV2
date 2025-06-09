using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Application.DTOs;

public class BudgetDto
{
    [Required(ErrorMessage = "Amount is required.")]
    public Guid UserId { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Limit is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Limit must be a positive number.")]
    public decimal Limit { get; set; }
    
    [Required(ErrorMessage = "Category is required.")]
    public Guid CategoryId { get; set; }
    
    public DateTime StartDate { get; set; } = DateTime.Now;
    
    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; }
}