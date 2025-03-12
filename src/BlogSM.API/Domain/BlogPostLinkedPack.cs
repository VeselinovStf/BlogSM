namespace BlogSM.API.Domain;

public class BlogPostLinkedPack
{
    public Guid BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }

    public Guid LinkedPackId { get; set; }
    public Pack LinkedPack { get; set; }
}
