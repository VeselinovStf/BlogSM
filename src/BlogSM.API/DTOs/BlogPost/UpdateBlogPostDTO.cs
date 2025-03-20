using System.ComponentModel.DataAnnotations;

namespace BlogSM.API.DTOs.BlogPost;

public class UpdateBlogPostDTO
{
    [Required]
    public Guid Id { get; set; }
    public Guid? LayoutId { get; set; }  // Match entity
    public string? URLTitle { get; set; }
    public string? Title { get; set; }
    public DateTime? Date { get; set; }
    public string? Preview { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? Alt { get; set; }
    public string? Short { get; set; }
    public string? Content { get; set; }
    public string? TopBanner { get; set; }
    public int? Discount { get; set; }

    public List<Guid>? CategoryIds { get; set; }  // Match entity
    public Guid? AuthorId { get; set; }  // Match entity
    public List<Guid>? TagIds { get; set; }  // Match entity
    public List<Guid>? LinkedPackIds { get; set; }  // Match entity
    public List<Guid>? DemoPackIds { get; set; }  // Match entity
    public bool? ViewTitle { get; set; }
    public Guid? PostTargetId { get; set; }  // Match entity
    public Guid? PageTypeId { get; set; }  // Match entity
}
