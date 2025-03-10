namespace BlogSM.API.Domain;

public class Tag : Entity
{
    public string Name { get; set; }

    public ICollection<BlogPost> BlogPosts { get; set; }
}
