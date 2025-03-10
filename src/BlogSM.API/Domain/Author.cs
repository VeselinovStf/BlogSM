namespace BlogSM.API.Domain;

public class Author : Entity
{
    public string Name {get;set;}

    public ICollection<BlogPost> BlogPosts { get; set; }
}