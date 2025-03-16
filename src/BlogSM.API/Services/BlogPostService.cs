using BlogSM.API.Domain;
using BlogSM.API.Persistence.Repositories.Abstraction;
using BlogSM.API.Services.Abstraction;
using BlogSM.API.Services.Models;

namespace BlogSM.API.Services;

public class BlogPostService(
    IBlogPostRepository blogPostRepo,
    ICategoryRepository categoryRepo,
    ITagRepository tagRepo,
    ILogger<BlogPostService> logger) : IBlogPostService
{
    private readonly IBlogPostRepository _blogPostRepo = blogPostRepo;
    private readonly ICategoryRepository _categoryRepo = categoryRepo;
    private readonly ITagRepository _tagRepo = tagRepo;
    private readonly ILogger<BlogPostService> _logger = logger;

    public async Task<ServiceResponse<BlogPost>> Create(BlogPost blogPost)
    {
        var response = new ServiceResponse<BlogPost>(false);

        try
        {
            if (blogPost == null)
            {
                response.Message = "Blog post cannot be null.";
                return response;
            }

            if (blogPost.Categories == null || blogPost.Categories.Count == 0 || blogPost.Tags == null || blogPost.Tags.Count == 0)
            {
                response.Message = "Categories or tags cannot be empty.";
                return response;
            }

            // Business Logic - Ensure categories exist in DB
            var categories = await _categoryRepo
                .GetCategoriesByIdsAsync(blogPost.Categories.Select(e => e.Id));

            if (categories.Count() != blogPost.Categories.Count)
            {
                response.Message = "Some categories do not exist.";
                return response;
            }

            // Business Logic - Ensure tags exist in DB
            var tags = await _tagRepo
                .GetTagsByIdsAsync(blogPost.Tags.Select(bpt => bpt.Id));

            if (tags.Count() != blogPost.Tags.Count)
            {
                response.Message = "Some tags do not exist.";
                return response;
            }

            // TODO: Rest of this       
            // Layout
            // Author
            // LinkedPack
            // PostTarget
            // PageType
            // Image - is existing at all - TODO: How images are imported to the main project

            blogPost.Categories = categories.ToList();
            blogPost.Tags = tags.ToList();

            await _blogPostRepo.AddAsync(blogPost);
            await _blogPostRepo.SaveAsync();

            response.Success = true;
            response.Message = "Blog post created successfully.";
            response.Data = blogPost;


        }
        catch (Exception ex)
        {
            response.Message = "An error occurred while saving the blog post.";
            response.Success = false;
            response.Data = null;

            _logger.LogError($"An error occurred while saving the blog post: {ex.Message}");
        }

        return response;
    }

    public async Task<ServiceResponse<BlogPost>> Get(Guid id)
    {
        var response = new ServiceResponse<BlogPost>(false);

        try
        {
            var blogPost = await _blogPostRepo.GetPostWithCategoriesAndTagsAsync(id);

            if (blogPost == null)
            {
                response.Message = $"Blog post not found";
                return response;
            }

            response.Success = true;
            response.Message = "Blog post retrieved successfully.";
            response.Data = blogPost;
        }
        catch (Exception ex)
        {
            response.Message = "An error occurred while retrieving the blog post";
            response.Success = false;
            response.Data = null;

            _logger.LogError($"An error occurred while retrieving the blog post: {ex.Message}");
        }

        return response;
    }
}
