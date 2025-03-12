namespace BlogSM.API.Domain;

public class BlogPostDemoPack
{
    public Guid BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; }

    public Guid DemoPackId { get; set; }
    public Pack DemoPack { get; set; }
}