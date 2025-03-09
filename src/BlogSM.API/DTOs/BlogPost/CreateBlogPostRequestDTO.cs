using System.ComponentModel.DataAnnotations;

namespace BlogSM.API.DTOs.BlogPost;

public class CreateBlogPostRequestDTO
{
    [Required]
    public Guid LayoutId { get; set; }  // Match entity

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
    public List<Guid> CategoryIds { get; set; }  // Match entity

    [Required]
    public Guid AuthorId { get; set; }  // Match entity

    [Required]
    public string Short { get; set; }

    [Required]
    public List<Guid> TagIds { get; set; }  // Match entity

    public List<Guid>? LinkedPackIds { get; set; }  // Match entity
    public List<Guid>? DemoPackIds { get; set; }  // Match entity

    [Required]
    public bool ViewTitle { get; set; }
    public string? TopBanner { get; set; }
    public int Discount { get; set; }

    [Required]
    public Guid PostTargetId { get; set; }  // Match entity
    [Required]
    public Guid PageTypeId { get; set; }  // Match entity

    [Required]
    public string Content { get; set; }
}
