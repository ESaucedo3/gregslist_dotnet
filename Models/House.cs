using System.ComponentModel.DataAnnotations;

namespace gregslist_csharp.Models;

public class House
{
  public int Id { get; set; }

  [Range(100, 10000)]
  public int? Sqft { get; set; }
  [Range(1, 30)]
  public int? Bedrooms { get; set; }
  [Range(1, 13)]
  public double? Bathrooms { get; set; }

  [MaxLength(255)]
  public string ImgUrl { get; set; }
  public string Description { get; set; }
  public int? Price { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public string CreatorId { get; set; }
  public Account Creator { get; set; }
}