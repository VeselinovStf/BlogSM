namespace BlogSM.API.Domain;

public class Category : Entity
{
    public string Name { get; set; }

    public List<BlogPost> BlogPosts { get;  set; } 
}