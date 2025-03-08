using System.ComponentModel.DataAnnotations;

namespace BlogSM.API.DTOs.BlogPost;

public class CreateBlogPostRequestDTO
{
    [Required]
    public string Layout { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Preview { get; set; }

    [Required]
    public string Image { get; set; }

    public string? Description { get; set; }

    [Required]
    public string? Alt { get; set; }

    [Required]
    public List<string> Categories { get; set; }

    [Required]
    public string Author { get; set; }

    [Required]
    public string Short { get; set; }

    [Required]
    public List<string> Tags { get; set; }

    public List<string>? LinkedPacks { get; set; }
    public List<string>? DemoPacks { get; set; }

    [Required]
    public bool ViewTitle { get; set; }
    public string? TopBanner { get; set; }
    public int Discount { get; set; }
    public string? PostTarget { get; set; }
    public string? PageType { get; set; }

    [Required]
    public string Content { get; set; }
}
