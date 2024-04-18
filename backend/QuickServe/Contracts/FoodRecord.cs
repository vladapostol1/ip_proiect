namespace QuickServe.Contracts;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Reprezinta un fel de mancare cu detaliile specifice
/// </summary>
public record class FoodRecord(
    int Id,
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(50)] string Category,
    [MaxLength(500)] string Description,
    [Range(0.01, 10000.00)] decimal Price,
    [Url] string ImagePath
);
