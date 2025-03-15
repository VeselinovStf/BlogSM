using Asp.Versioning;

using AutoMapper;

using BlogSM.API.Domain;
using BlogSM.API.DTOs.BlogPost;
using BlogSM.API.Services.Abstraction;

using Microsoft.AspNetCore.Mvc;

namespace BlogSM.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BlogPostController(IMapper mapper, IBlogPostService blogPostService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IBlogPostService _blogPostService = blogPostService;

        [HttpGet("{blogPostId:guid}")]
        public async Task<IActionResult> Get(Guid blogPostId)
        {
            // USECASE - GET PRODUCT
            var blogPost = await _blogPostService.Get(blogPostId);

            if(!blogPost.Success){
                return NotFound(new { blogPost.Message });
            }

            // MAP TO EXTERNAL REPRESENTATION
            var blogPostResponseModel = _mapper.Map<BlogPostResponseDTO>(blogPost.Data);

            return Ok(blogPostResponseModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogPostRequestDTO createBlogPostRequest)
        {
            // MAP TO INTERNAL REPRESENTATION
            var newBlogPost = _mapper.Map<BlogPost>(createBlogPostRequest);

            // USECASE - CREATE PRODUCT
            var createdBlogPost = await _blogPostService.Create(newBlogPost);

            if (!createdBlogPost.Success)
            {
                return BadRequest(new { message = createdBlogPost.Message });
            }

            // MAP TO EXTERNAL REPRESENTATION
            var createdBlogPostResponseModel = _mapper.Map<BlogPostResponseDTO>(createdBlogPost.Data);

            // RETURN RESULT
            return CreatedAtAction(
                nameof(Get),
                new { BlogPostId = createdBlogPostResponseModel.Id },
                new
                {
                    success = createdBlogPost.Success,
                    message = createdBlogPost.Message,
                    data = createdBlogPostResponseModel
                });
        }

        // TODO: Update/Put

        // TODO: Delete
        
        // TODO: Publish/Unpublish - Contains logic for moving the actual post to the SITE
    }
}
