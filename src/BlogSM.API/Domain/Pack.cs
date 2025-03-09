namespace BlogSM.API.Domain;

public class Pack : Entity
{
    public string Name { get; set; }

    public List<BlogPost> LinkedBlogPosts { get;  set; }
    public List<BlogPost> DemoBlogPosts { get;  set; } 
}
