using Asp.Versioning;

using BlogSM.API.DTOs.BlogPost;

using Microsoft.AspNetCore.Mvc;

namespace BlogSM.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BlogPostController : ControllerBase
    {
        [HttpGet("{blogPostId:guid}")]
        public IActionResult Get(Guid blogPostId){

            // get 

            return Ok(new { Message = "Working..."});
        }

        [HttpPost]
        public IActionResult Create(CreateBlogPostRequestDTO createBlogPostRequest){

            // create

            return CreatedAtAction(
                nameof(Get),
                new { BlogPostId = Guid.NewGuid()},
                createBlogPostRequest
            );
        }
    }
}
