using System;

using BlogSM.API.Domain;

namespace BlogSM.API.Services;

public class BlogPostService
{
    private static readonly List<BlogPost> InMemmoryBlogPosts = new List<BlogPost>();
    public void Create(BlogPost blogPost){
        // Business Logic - Store to Storage


        InMemmoryBlogPosts.Add(blogPost);
    }

    public BlogPost? Get(Guid id){
        // Business Logic - Get from Storage

        return InMemmoryBlogPosts.FirstOrDefault(p => p.Id == id);
    }
}
