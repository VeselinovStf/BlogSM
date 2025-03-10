namespace BlogSM.API.Domain;

public class Category : Entity
{
    public string Name { get; set; }

    public ICollection<BlogPost> BlogPosts { get;  set; } 
}