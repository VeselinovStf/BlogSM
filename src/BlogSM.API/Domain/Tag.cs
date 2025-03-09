namespace BlogSM.API.Domain;

public class Tag : Entity
{
    public string Name { get; set; }

    public List<BlogPost> BlogPosts { get; set; }
}
