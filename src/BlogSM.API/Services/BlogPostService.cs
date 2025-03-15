using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;
using BlogSM.API.Services.Abstraction;
using BlogSM.API.Services.Models;

using Microsoft.EntityFrameworkCore;

namespace BlogSM.API.Services;

public class BlogPostService(
    IBlogPostRepository blogPostRepo, 
    ICategoryRepository categoryRepo,
    ITagRepository tagRepo) : IBlogPostService
{
    private readonly IBlogPostRepository _blogPostRepo = blogPostRepo;
    private readonly ICategoryRepository _categoryRepo = categoryRepo;
    private readonly ITagRepository _tagRepo = tagRepo;

    public async Task<ServiceResponse<BlogPost>> Create(BlogPost blogPost)
    {
        var response = new ServiceResponse<BlogPost>(false);

        var categories = await _categoryRepo
            .GetCategoriesByIds(blogPost.Categories.Select(e => e.Id))
            .ToListAsync();

        if (categories.Count != blogPost.Categories.Count)
        {
            response.Message = "Some categories do not exist.";
            return response;
        }

        // Business Logic - Ensure tags exist in DB
        var tags = await _tagRepo.GetTagsByIds(blogPost.Tags.Select(bpt => bpt.Id))
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
        // Image - is existing at all - TODO: How images are imported to the main project

        blogPost.Categories = categories;
        blogPost.Tags = tags;

        await _blogPostRepo.AddAsync(blogPost);
        await _blogPostRepo.SaveAsync();

        response.Success = true;
        response.Message = "Blog post created successfully.";
        response.Data = blogPost;

        return response;
    }

    public async Task<ServiceResponse<BlogPost>> Get(Guid id)
    {
        var blogPost = await _blogPostRepo.GetPostWithCategoriesAndTagsAsync(id);

        if (blogPost == null)
        {
            return new ServiceResponse<BlogPost>(false, $"Blog Post with id: {id} is not found");
        }

        return new ServiceResponse<BlogPost>(true, "Blog post retrieved successfully.", blogPost);
    }
}
