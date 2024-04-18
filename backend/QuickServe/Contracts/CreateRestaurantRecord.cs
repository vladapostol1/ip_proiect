namespace QuickServe;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CreateRestaurantRecord
{
    [Required, MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [Required, MaxLength(200)]
    public string Address { get; init; } = string.Empty;

    [Url, MaxLength(255)]
    public string Website { get; init; } = string.Empty;

    [Required, MaxLength(50)]
    public string Username { get; init; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    public List<CreateFoodRecord> MenuItems { get; init; } = new List<CreateFoodRecord>();
}
