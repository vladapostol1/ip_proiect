namespace QuickServe;
using System.ComponentModel.DataAnnotations;
using QuickServe.Contracts;

/// <summary>
/// Reprezinta datele unui utilizator
/// </summary>
public record class RestaurantRecord(
    [Required, MaxLength(1000)] int Id,
    [Required, MaxLength(100)] string Name,
    [Required, MaxLength(200)] string Address,
    [Url] string Website,
    List<FoodRecord> MenuItems
);
