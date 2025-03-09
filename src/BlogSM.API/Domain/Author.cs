namespace BlogSM.API.Domain;

public class Author : Entity
{
    public string Name {get;set;}

    public List<BlogPost> BlogPosts { get; set; }
}