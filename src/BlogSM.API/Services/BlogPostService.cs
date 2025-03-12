using System;

using BlogSM.API.Domain;
using BlogSM.API.Persistence;

namespace BlogSM.API.Services;

public class BlogPostService(BlogSMDbContext blogSMDbContext)
{
    private readonly BlogSMDbContext _blogSMDbContext = blogSMDbContext;

    public void Create(BlogPost blogPost){
        // Business Logic - Store to Storage

        _blogSMDbContext.BlogPosts.Add(blogPost);
        _blogSMDbContext.SaveChanges();
    }

    public BlogPost? Get(Guid id){
        // Business Logic - Get from Storage

        return _blogSMDbContext.BlogPosts.FirstOrDefault(p => p.Id == id);
    }
}
