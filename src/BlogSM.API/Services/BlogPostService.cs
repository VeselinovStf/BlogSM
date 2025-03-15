using BlogSM.API.Domain;
using BlogSM.API.Persistence;
using BlogSM.API.Services.Models;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Services;

public class BlogPostService(BlogSMDbContext blogSMDbContext)
{
    private readonly BlogSMDbContext _blogSMDbContext = blogSMDbContext;

    public async Task<ServiceResponse<BlogPost>> Create(BlogPost blogPost)
    {
        var response = new ServiceResponse<BlogPost>(false);

        var categories = await _blogSMDbContext.Categorys
                    .Where(c => blogPost.Categories.Select(e => e.Id).Contains(c.Id))
                    .ToListAsync();

        if (categories.Count != blogPost.Categories.Count)
        {
            response.Message = "Some categories do not exist.";
            return response;
        }

        // Business Logic - Ensure tags exist in DB
        var tags = await _blogSMDbContext.Tags
            .Where(t => blogPost.Tags.Select(bpt => bpt.Id).Contains(t.Id))
            .ToListAsync();

        if (tags.Count != blogPost.Tags.Count)
        {
            response.Message = "Some tags do not exist.";
            return response;
        }

        // TODO: Rest of this
        // Business Logic - Ensure categories exist in DB
        // Layout
        // Author
        // LinkedPack
        // PostTarget
        // PageType
        // Image - is existing at all - TODO: How images are imported to the mayne projec

        blogPost.Categories = categories;
        blogPost.Tags = tags;

        await _blogSMDbContext.BlogPosts.AddAsync(blogPost);
        await _blogSMDbContext.SaveChangesAsync();

        response.Success = true;
        response.Message = "Blog post created successfully.";
        response.Data = blogPost;

        return response;
    }

    public async Task<ServiceResponse<BlogPost>> Get(Guid id)
    {
        var blogPost = await _blogSMDbContext.BlogPosts
            .Include(bp => bp.Categories) // TODO: Take only Id's
            .Include(bp => bp.Tags)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (blogPost == null)
        {
            return new ServiceResponse<BlogPost>(false, $"Blog Post with id: {id} is not found");
        }

        return new ServiceResponse<BlogPost>(true, "Blog post retrieved successfully.", blogPost);
    }
}
