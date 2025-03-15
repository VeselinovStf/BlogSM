using System.ComponentModel.DataAnnotations;

namespace BlogSM.API.DTOs.BlogPost;

public class CreateBlogPostRequestDTO
{
    [Required]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "URLTitle must be between 5 and 80 characters.")]
    public string URLTitle { get; set; }

    [Required]
    public Guid LayoutId { get; set; }  // Match entity

    [Required]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 80 characters.")]
    public string Title { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 80 characters.")]
    public string Preview { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Image must be between 5 and 80 characters.")]
    public string Image { get; set; }

    public string? Description { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Alt must be between 5 and 80 characters.")]
    public string Alt { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one CategoryIds is required.")]
    public List<Guid> CategoryIds { get; set; }  // Match entity

    [Required]
    public Guid AuthorId { get; set; }  // Match entity

    [Required]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Short must be between 5 and 80 characters.")]
    public string Short { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one TagId is required.")]
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
    [MinLength( 5, ErrorMessage = "Content must be at least 5 characters.")]
    public string Content { get; set; }
}
