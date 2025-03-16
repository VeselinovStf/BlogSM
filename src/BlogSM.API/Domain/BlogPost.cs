namespace BlogSM.API.Domain;

public class BlogPost : Entity
{
    public bool IsPublished { get; set; }
    public string URLTitle { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Preview { get; set; }
    public string Image { get; set; }
    public string? Description { get; set; }
    public string? Alt { get; set; }
    public string Short { get; set; }
    public bool ViewTitle { get; set; }
    public string? TopBanner { get; set; }
    public int Discount { get; set; }
    public string Content { get; set; }

    public Guid PostTargetId { get; set; }
    public PostTarget PostTarget { get; set; }
    public Guid PageTypeId { get; set; }
    public PageType PageType { get; set; }
    public Guid AuthorId { get; set; }
    public Author Author { get; set; }
    public Guid LayoutId { get; set; }
    public Layout Layout { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public ICollection<Pack> LinkedPacks { get; set; } = new HashSet<Pack>();
    public ICollection<Pack> DemoPacks { get; set; } = new HashSet<Pack>();
    public ICollection<Category> Categories { get; set; }

}
