using BlogSM.API.Domain;
using BlogSM.API.DTOs.BlogPost;
using BlogSM.API.Persistence.Repositories.Abstraction;
using BlogSM.API.Services.Abstraction;
using BlogSM.API.Services.Models;
using BlogSM.API.Utility;

namespace BlogSM.API.Services;

public class BlogPostService(
    IBlogPostRepository blogPostRepo,
    ICategoryRepository categoryRepo,
    ITagRepository tagRepo,
    IAuthorRepository authorRepo,
    ILayoutRepository layoutRepo,
    IPackRepository packRepo,
    IPageTypeRepository pageTypeRepo,
    IPostTargetRepository postTargetRepo,
    ILogger<BlogPostService> logger) : IBlogPostService
{
    private readonly IBlogPostRepository _blogPostRepo = blogPostRepo;
    private readonly ICategoryRepository _categoryRepo = categoryRepo;
    private readonly ITagRepository _tagRepo = tagRepo;
    private readonly IAuthorRepository _authorRepo = authorRepo;
    private readonly ILayoutRepository _layoutRepo = layoutRepo;
    private readonly IPackRepository _packRepo = packRepo;
    private readonly IPageTypeRepository _pageTypeRepo = pageTypeRepo;
    private readonly IPostTargetRepository _postTargetRepo = postTargetRepo;
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

            if (blogPost.Categories.Select(c => c.Id).ToHashSet().Count() != blogPost.Categories.Count())
            {
                response.Message = "Categories must have unique IDs.";
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

            if (blogPost.Tags.Select(c => c.Id).ToHashSet().Count() != blogPost.Tags.Count())
            {
                response.Message = "Tags must have unique IDs.";
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

            // Business Logic - Ensure Layout exist in DB
            var layout = await _layoutRepo.GetByIdAsync(blogPost.LayoutId);
            if (layout == null)
            {
                response.Message = "Layout is missing.";
                return response;
            }

            // Business Logic - Ensure Author exist in DB
            var author = await _authorRepo.GetByIdAsync(blogPost.AuthorId);
            if (author == null)
            {
                response.Message = "Author is missing.";
                return response;
            }

            // Business Logic - Ensure Packs for LinkedPack/DemoPacks exist in DB
            if (blogPost.LinkedPacks.Any() || blogPost.DemoPacks.Any())
            {
                var linkedPacksIds = blogPost.LinkedPacks.Select(p => p.Id).ToHashSet();
                if (linkedPacksIds.Count() != blogPost.LinkedPacks.Count())
                {
                    response.Message = "Can't use duplicated packs in Linked Packs";
                    return response;
                }

                var demoPackIds = blogPost.DemoPacks.Select(p => p.Id).ToHashSet();
                if (demoPackIds.Count() != blogPost.DemoPacks.Count())
                {
                    response.Message = "Can't use duplicated packs in Demo Packs";
                    return response;
                }

                var combinedSearchedPacksId = blogPost.LinkedPacks
                    .Concat(blogPost.DemoPacks)
                    .Select(c => c.Id);

                var packs = await _packRepo.GetAllByIdsAsync(combinedSearchedPacksId);

                var packsDictionary = packs.ToDictionary(p => p.Id);

                var linkedPacks = linkedPacksIds.Where(lp => packsDictionary.ContainsKey(lp))
                    .Select(lp => packsDictionary[lp])
                    .ToList();

                if (linkedPacks.Count() != blogPost.LinkedPacks.Count)
                {
                    response.Message = "Some Linked Packs do not exist.";
                    return response;
                }

                blogPost.LinkedPacks = linkedPacks.ToList();
            
                var demoPacks = demoPackIds.Where(dp => packsDictionary.ContainsKey(dp))
                    .Select(dp => packsDictionary[dp])
                    .ToList();

                if (demoPacks.Count() != blogPost.DemoPacks.Count)
                {
                    response.Message = "Some Demo Packs do not exist.";
                    return response;
                }

                blogPost.DemoPacks = demoPacks.ToList();

            }

            // Business Logic - Ensure PostTarget exist in DB
            var postTarget = await _postTargetRepo.GetByIdAsync(blogPost.PostTargetId);
            if (postTarget == null)
            {
                response.Message = "PostTarget is missing.";
                return response;
            }

            // Business Logic - Ensure PostTarget exist in DB
            var pageType = await _pageTypeRepo.GetByIdAsync(blogPost.PageTypeId);
            if (pageType == null)
            {
                response.Message = "PageType is missing.";
                return response;
            }

            // Validate TopBanner
            if (!string.IsNullOrEmpty(blogPost.TopBanner))
            {
                if (!Guard.IsValidStringLength(blogPost.TopBanner.Length, 5, 80))
                {
                    response.Message = "TopBanner must be between 5 to 80 characters long.";
                    return response;
                }
            }

            // Validate Discount
            if (blogPost.Discount < 0 || blogPost.Discount > 100)
            {
                response.Message = "Discount must be between 0 and 100.";
                return response;
            }


            // Image
            // TODO: How images are imported to the main project
            // Every Post is going to have its own `Setting` that  is going to point to witch `Project` is related
            // The `Setting` is going to contain the in/out folders, same `Setting` is going to be used for Publishing

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

            _logger.LogError($"An error occurred while saving the blog post: {ex.Message} : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
        }

        return response;
    }

    public async Task<ServiceResponse<BlogPost>> Get(Guid id)
    {
        var response = new ServiceResponse<BlogPost>(false);

        try
        {
            var blogPost = await _blogPostRepo.GetPostWithRelationIdsAsync(id);

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

            _logger.LogError($"An error occurred while retrieving the blog post: {ex.Message} : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
        }

        return response;
    }

    public async Task<ServiceResponse<BlogPost>> Update(UpdateBlogPostDTO blogPostUpdateDTO)
    {
        var response = new ServiceResponse<BlogPost>(false);

        try
        {
            if (blogPostUpdateDTO == null)
            {
                response.Message = "Blog post cannot be null.";
                return response;
            }

            // Fetch the existing BlogPost from the database
            var existingBlogPost = await _blogPostRepo.GetPostWithIncludesAsync(blogPostUpdateDTO.Id);
            if (existingBlogPost == null)
            {
                response.Message = "Blog post not found.";
                return response;
            }

            // TODO: How to update published post ! In app case published means send to SSG
            // Publish/Edit Published/Delete Published - CRUD - Publish
            if (existingBlogPost.IsPublished)
            {
                response.Message = "Can't Edit Published Post.";
                return response;
            }

            if (blogPostUpdateDTO.LayoutId.HasValue && blogPostUpdateDTO.LayoutId.Value != existingBlogPost.LayoutId)
            {
                // Business Logic - Ensure Layout exists in DB
                var layout = await _layoutRepo.GetByIdAsync(blogPostUpdateDTO.LayoutId.Value);
                if (layout == null)
                {
                    response.Message = "Layout Id is missing.";
                    return response;
                }

                existingBlogPost.LayoutId = blogPostUpdateDTO.LayoutId.Value;
            }

            if (blogPostUpdateDTO.AuthorId.HasValue && blogPostUpdateDTO.AuthorId.Value != existingBlogPost.AuthorId)
            {
                // Business Logic - Ensure Author exists in DB
                var author = await _authorRepo.GetByIdAsync(blogPostUpdateDTO.AuthorId.Value);
                if (author == null)
                {
                    response.Message = "Author Id is missing.";
                    return response;
                }

                existingBlogPost.AuthorId = blogPostUpdateDTO.AuthorId.Value;
            }

            if (blogPostUpdateDTO.CategoryIds != null)
            {
                var newCategoryUniqueIds = blogPostUpdateDTO.CategoryIds.ToHashSet();

                if (newCategoryUniqueIds.Count() != blogPostUpdateDTO.CategoryIds.Count())
                {
                    response.Message = "Categories must have unique IDs.";
                    return response;
                }

                var existingCategoryUniqueIds = existingBlogPost.Categories.Select(c => c.Id).ToHashSet();

                if (!existingCategoryUniqueIds.SetEquals(newCategoryUniqueIds))
                {
                    // Business Logic - Ensure categories exist in DB
                    var categories = await _categoryRepo.GetCategoriesByIdsAsync(blogPostUpdateDTO.CategoryIds);
                    if (categories.Count() != blogPostUpdateDTO.CategoryIds.Count)
                    {
                        response.Message = "Some categories do not exist.";
                        return response;
                    }

                    existingBlogPost.Categories = categories.ToList();
                }
            }

            if (blogPostUpdateDTO.TagIds != null)
            {
                var newTagUniqueIds = blogPostUpdateDTO.TagIds.ToHashSet();

                if (newTagUniqueIds.Count() != blogPostUpdateDTO.TagIds.Count())
                {
                    response.Message = "Tags must have unique IDs.";
                    return response;
                }

                var existingTagUniqueIds = existingBlogPost.Tags.Select(c => c.Id).ToHashSet();

                if (!existingTagUniqueIds.SetEquals(newTagUniqueIds))
                {
                    // Business Logic - Ensure tags exist in DB
                    var tags = await _tagRepo.GetTagsByIdsAsync(blogPostUpdateDTO.TagIds);
                    if (tags.Count() != blogPostUpdateDTO.TagIds.Count)
                    {
                        response.Message = "Some tags do not exist.";
                        return response;
                    }

                    existingBlogPost.Tags = tags.ToList();
                }
            }

            if (blogPostUpdateDTO.PostTargetId.HasValue && blogPostUpdateDTO.PostTargetId.Value != existingBlogPost.PostTargetId)
            {
                // Business Logic - Ensure PostTarget exists in DB
                var postTarget = await _postTargetRepo.GetByIdAsync(blogPostUpdateDTO.PostTargetId.Value);
                if (postTarget == null)
                {
                    response.Message = "PostTarget is missing.";
                    return response;
                }

                existingBlogPost.PostTargetId = blogPostUpdateDTO.PostTargetId.Value;
            }

            if (blogPostUpdateDTO.PageTypeId.HasValue && blogPostUpdateDTO.PageTypeId.Value != existingBlogPost.PageTypeId)
            {
                // // Business Logic - Ensure PageType exists in DB
                var pageType = await _pageTypeRepo.GetByIdAsync(blogPostUpdateDTO.PageTypeId.Value);
                if (pageType == null)
                {
                    response.Message = "PageType is missing.";
                    return response;
                }

                existingBlogPost.PageTypeId = blogPostUpdateDTO.PageTypeId.Value;
            }

            if (blogPostUpdateDTO.ViewTitle.HasValue && blogPostUpdateDTO.ViewTitle.Value != existingBlogPost.ViewTitle)
            {
                existingBlogPost.ViewTitle = blogPostUpdateDTO.ViewTitle.Value;
            }

            if (blogPostUpdateDTO.LinkedPackIds != null)
            {
                var newLinkePackUniqIds = blogPostUpdateDTO.LinkedPackIds.ToHashSet();

                if (newLinkePackUniqIds.Count() != blogPostUpdateDTO.LinkedPackIds.Count())
                {
                    response.Message = "Linked Packs must have unique IDs.";
                    return response;
                }

                var existingLinkedPackUniqIds = existingBlogPost.LinkedPacks.Select(p => p.Id).ToHashSet();

                if (!existingLinkedPackUniqIds.SetEquals(newLinkePackUniqIds))
                {
                    var packs = await _packRepo.GetAllByIdsAsync(newLinkePackUniqIds);

                    if (packs.Count() != newLinkePackUniqIds.Count())
                    {
                        response.Message = "Some Linked Packs do not exist.";
                        return response;
                    }

                    existingBlogPost.LinkedPacks = packs.ToList();
                }
            }

            // Validate Demo Packs
            if (blogPostUpdateDTO.DemoPackIds != null)
            {
                var newDemoPackskUniqIds = blogPostUpdateDTO.DemoPackIds.ToHashSet();

                if (newDemoPackskUniqIds.Count() != blogPostUpdateDTO.DemoPackIds.Count())
                {
                    response.Message = "Demo Packs must have unique IDs.";
                    return response;
                }

                var existingDemoPackskUniqIds = existingBlogPost.DemoPacks.Select(p => p.Id).ToHashSet();

                if (!existingDemoPackskUniqIds.SetEquals(newDemoPackskUniqIds))
                {
                    var packs = await _packRepo.GetAllByIdsAsync(newDemoPackskUniqIds);

                    if (packs.Count() != newDemoPackskUniqIds.Count())
                    {
                        response.Message = "Some Demo Packs do not exist.";
                        return response;
                    }

                    existingBlogPost.DemoPacks = packs.ToList();
                }
            }

            // Validate URLTitle
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.URLTitle) && blogPostUpdateDTO.URLTitle != existingBlogPost.URLTitle)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.URLTitle.Length, 5, 80))
                {
                    response.Message = "URLTitle must be between 5 and 80 characters.";
                    return response;
                }

                existingBlogPost.URLTitle = blogPostUpdateDTO.URLTitle;
            }

            // Validate Title
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.Title) && blogPostUpdateDTO.Title != existingBlogPost.Title)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.Title.Length, 5, 80))
                {
                    response.Message = "Title must be between 5 and 80 characters.";
                    return response;
                }

                existingBlogPost.Title = blogPostUpdateDTO.Title;
            }

            // Validate Date
            if (blogPostUpdateDTO.Date.HasValue && blogPostUpdateDTO.Date.Value != existingBlogPost.Date)
            {
                existingBlogPost.Date = blogPostUpdateDTO.Date.Value;
            }

            // Validate Preview
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.Preview) && blogPostUpdateDTO.Preview != existingBlogPost.Preview)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.Preview.Length, 5, 80))
                {
                    response.Message = "Preview must be between 5 to 80 characters long.";
                    return response;
                }

                existingBlogPost.Preview = blogPostUpdateDTO.Preview;
            }

            // Validate Image 
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.Image) && blogPostUpdateDTO.Image != existingBlogPost.Image)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.Image.Length, 5, 80))
                {
                    response.Message = "Image must be between 5 and 80 characters.";
                    return response;
                }

                existingBlogPost.Image = blogPostUpdateDTO.Image;
            }

            // Validate Description
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.Description) && blogPostUpdateDTO.Description != existingBlogPost.Description)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.Description.Length, 5, 80))
                {
                    response.Message = "Description must be between 5 to 80  characters long.";
                    return response;
                }

                existingBlogPost.Description = blogPostUpdateDTO.Description;
            }

            // Validate Alt (non-empty, for example)
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.Alt) && blogPostUpdateDTO.Alt != existingBlogPost.Alt)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.Alt.Length, 5, 80))
                {
                    response.Message = "Alt must be between 5 to 80 characters long.";
                    return response;
                }

                existingBlogPost.Alt = blogPostUpdateDTO.Alt;
            }

            // Validate Short (non-empty, for example)
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.Short) && blogPostUpdateDTO.Short != existingBlogPost.Short)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.Short.Length, 5, 80))
                {
                    response.Message = "Short must be between 5 to 80 characters long.";
                    return response;
                }

                existingBlogPost.Short = blogPostUpdateDTO.Short;
            }

            // Validate Content
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.Content) && blogPostUpdateDTO.Content != existingBlogPost.Content)
            {
                if (blogPostUpdateDTO.Content.Length < 5)
                {
                    response.Message = "Content must be between 5 to 80 characters long.";
                    return response;
                }

                existingBlogPost.Content = blogPostUpdateDTO.Content;
            }

            // Validate TopBanner
            if (!string.IsNullOrEmpty(blogPostUpdateDTO.TopBanner) && blogPostUpdateDTO.TopBanner != existingBlogPost.TopBanner)
            {
                if (!Guard.IsValidStringLength(blogPostUpdateDTO.TopBanner.Length, 5, 80))
                {
                    response.Message = "TopBanner must be between 5 to 80 characters long.";
                    return response;
                }

                existingBlogPost.TopBanner = blogPostUpdateDTO.TopBanner;
            }

            // Validate Discount
            if (blogPostUpdateDTO.Discount.HasValue && blogPostUpdateDTO.Discount.Value != existingBlogPost.Discount)
            {
                if (blogPostUpdateDTO.Discount < 0 || blogPostUpdateDTO.Discount > 100)
                {
                    response.Message = "Discount must be between 0 and 100.";
                    return response;
                }

                existingBlogPost.Discount = blogPostUpdateDTO.Discount.Value;
            }

            // Save changes
            _blogPostRepo.Update(existingBlogPost);
            await _blogPostRepo.SaveAsync();

            response.Success = true;
            response.Message = "Blog post updated successfully.";
            response.Data = existingBlogPost;
        }
        catch (Exception ex)
        {
            response.Message = "An error occurred while updating the blog post.";
            response.Success = false;
            response.Data = null;

            _logger.LogError($"An error occurred while updating the blog post with ID: {blogPostUpdateDTO.Id} : {ex.Message} : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
        }

        return response;
    }

    public async Task<ServiceResponse<BlogPost>> Delete(Guid id)
    {
        var response = new ServiceResponse<BlogPost>(false);

        try
        {
            var blogPost = await _blogPostRepo.GetByIdAsync(id);

            if (blogPost == null)
            {
                response.Message = $"Blog post not found";
                return response;
            }

            if (blogPost.IsPublished)
            {
                response.Message = "Can't Delete Published Post.";
                return response;
            }

            _blogPostRepo.Delete(blogPost);
            await _blogPostRepo.SaveAsync();

            response.Success = true;
            response.Message = "Blog post deleted successfully.";
            response.Data = blogPost;
        }
        catch (Exception ex)
        {
            response.Message = "An error occurred while deleting the blog post";
            response.Success = false;
            response.Data = null;

            _logger.LogError($"An error occurred while deleting blog post: {ex.Message} : {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}");
        }

        return response;
    }
}
