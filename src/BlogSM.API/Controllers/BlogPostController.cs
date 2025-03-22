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

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] Guid? tagId = null,
            [FromQuery] Guid? authorId = null
        )
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { message = "Page and PageSize must be greater than 0." });
            }

            var serviceResponse = await _blogPostService.GetAll(page, pageSize, sortOrder, search, sortBy, categoryId, tagId, authorId);

            if (!serviceResponse.Success)
            {
                return BadRequest(new { message = serviceResponse.Message });
            }

            var blogPostsResponseModel = _mapper.Map<IEnumerable<BlogPostResponseDTO>>(serviceResponse.Data);

            return Ok(new
            {
                success = serviceResponse.Success,
                message = serviceResponse.Message,
                data = blogPostsResponseModel
            });
        }

        [HttpGet("{blogPostId:guid}")]
        public async Task<IActionResult> Get(Guid blogPostId)
        {
            // USECASE - GET PRODUCT
            var blogPost = await _blogPostService.Get(blogPostId);

            if (!blogPost.Success)
            {
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

        [HttpPut("{blogPostId:guid}")]
        public async Task<IActionResult> Update(Guid blogPostId, [FromBody] UpdateBlogPostDTO updateBlogPostDTO)
        {
            if (blogPostId != updateBlogPostDTO.Id)
            {
                return BadRequest(new { message = "Blog Post Id mismatch" });
            }

            // USECASE - CREATE PRODUCT
            var updatedBlogPost = await _blogPostService.Update(updateBlogPostDTO);

            // TODO: Messages are hardcoded for now!!
            if (updatedBlogPost.Message == "Blog post not found")
            {
                return NotFound(new { message = updatedBlogPost.Message });
            }

            if (!updatedBlogPost.Success)
            {
                return BadRequest(new { message = updatedBlogPost.Message });
            }

            // MAP TO EXTERNAL REPRESENTATION
            var createdBlogPostResponseModel = _mapper.Map<BlogPostResponseDTO>(updatedBlogPost.Data);

            // RETURN RESULT
            return Ok(new
            {
                success = updatedBlogPost.Success,
                message = updatedBlogPost.Message,
                data = createdBlogPostResponseModel
            });
        }

        [HttpDelete("{blogPostId:guid}")]
        public async Task<IActionResult> Delete(Guid blogPostId)
        {
            var deleteBlogPostServiceResponse = await _blogPostService.Delete(blogPostId);

            if (!deleteBlogPostServiceResponse.Success)
            {
                // TODO: Messages are hardcoded for now!!
                if (deleteBlogPostServiceResponse.Message == "Blog post not found")
                {
                    return NotFound(new { message = deleteBlogPostServiceResponse.Message });
                }

                return BadRequest(new { message = deleteBlogPostServiceResponse.Message });
            }

            return NoContent();
        }

        // TODO: Publish/Unpublish - Contains logic for moving the actual post to the SITE
        // TODO: Settings - from where comes the settings for publishing ?? for starting out use appsettings?
    }
}
