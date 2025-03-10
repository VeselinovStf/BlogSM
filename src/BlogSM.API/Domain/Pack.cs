namespace BlogSM.API.Domain;

public class Pack : Entity
{
    public string Name { get; set; }

    public ICollection<BlogPost> LinkedBlogPosts { get;  set; }
    public ICollection<BlogPost> DemoBlogPosts { get;  set; } 
}
