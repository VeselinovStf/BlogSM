using Asp.Versioning;

using AutoMapper;

using BlogSM.API.Domain;
using BlogSM.API.DTOs.BlogPost;

using Microsoft.AspNetCore.Mvc;

namespace BlogSM.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BlogPostController(IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet("{blogPostId:guid}")]
        public IActionResult Get(Guid blogPostId){

            // get 

            return Ok(new { Message = "Working..."});
        }

        [HttpPost]
        public IActionResult Create(CreateBlogPostRequestDTO createBlogPostRequest)
        {

            // MAP TO INTERNAL REPRESENTATION
            var newBlogPost = _mapper.Map<BlogPost>(createBlogPostRequest);

            // CREATE PRODUCT

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
