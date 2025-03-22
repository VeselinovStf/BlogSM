using BlogSM.API.Domain;
using BlogSM.API.DTOs.BlogPost;
using BlogSM.API.Services.Models;

namespace BlogSM.API.Services.Abstraction;

public interface IBlogPostService
{
    Task<ServiceResponse<BlogPost>> Create(BlogPost blogPost);
    Task<ServiceResponse<BlogPost>> Get(Guid id);
    Task<ServiceResponse<BlogPost>> Update(UpdateBlogPostDTO blogPost);
    Task<ServiceResponse<BlogPost>> Delete(Guid id);
    Task<ServiceResponse<IEnumerable<BlogPost>>> GetAll(int page, int pageSize, string? sortOrder, string? search, string? sortBy, Guid? categoryId, Guid? tagId, Guid? authorId);
}
