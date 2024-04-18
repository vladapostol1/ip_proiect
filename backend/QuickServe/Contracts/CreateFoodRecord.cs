namespace QuickServe;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class CreateFoodRecord
{
    [Required, MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, MaxLength(50)]
    public string Category { get; init; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; init; } = string.Empty;

    [Range(0.01, 10000.00)]
    public decimal Price { get; init; }

    [Url, MaxLength(255)]
    public string ImagePath { get; init; } = string.Empty;
}