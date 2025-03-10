using Asp.Versioning;

using AutoMapper;

using BlogSM.API.Domain;
using BlogSM.API.DTOs.BlogPost;
using BlogSM.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace BlogSM.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BlogPostController(IMapper mapper, BlogPostService blogPostService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly BlogPostService _blogPostService = blogPostService;

        [HttpGet("{blogPostId:guid}")]
        public IActionResult Get(Guid blogPostId)
        {
            // USECASE - GET PRODUCT
            var blogPost = _blogPostService.Get(blogPostId);

            // MAP TO EXTERNAL REPRESENTATION
            var blogPostResponseModel = _mapper.Map<BlogPostResponseDTO>(blogPost);

            return blogPostResponseModel is null 
                ? Problem(statusCode: StatusCodes.Status404NotFound, detail: $"Blog Post with id: {blogPostId} is not found") 
                : Ok(blogPostResponseModel);
        }

        [HttpPost]
        public IActionResult Create(CreateBlogPostRequestDTO createBlogPostRequest)
        {

            // MAP TO INTERNAL REPRESENTATION
            var newBlogPost = _mapper.Map<BlogPost>(createBlogPostRequest);

            // USECASE - CREATE PRODUCT
            _blogPostService.Create(newBlogPost);

            // MAP TO EXTERNAL REPRESENTATION
            var createdBlogPostResponseModel = _mapper.Map<BlogPostResponseDTO>(newBlogPost);

            // RETURN RESULT
            return CreatedAtAction(
                nameof(Get),
                new { BlogPostId = createdBlogPostResponseModel.Id },
                createdBlogPostResponseModel
            );
        }
    }
}
