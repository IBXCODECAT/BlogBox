using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly DataContext context;

        public PostsController(DataContext context)
        {
            this.context = context;
        }

        //Get api/posts
        [HttpGet(Name = "GetPosts")]
        public ActionResult<List<Post>> Get()
        {
            return context.posts.ToList();
        }

        /// <summary>
        /// Get API/Posts/[id]
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <returns>A single post matching the specified id</returns>
        [HttpGet("{id}", Name = "GetPost")]
        public ActionResult<Post> GetPost(Guid id)
        {
            Post? post = this.context.posts.Find(id);

            if (post is null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpPost(Name = "Create")]
        public ActionResult<Post> Create([FromBody] Post request)
        {
            Post post = new Post
            {
                Id = request.Id,
                Title = request.Title,
                Body = request.Body,
                Date = request.Date
            };

            context.posts.Add(post);

            bool success = context.SaveChanges() > 0;

            if(success)
            {
                return Ok(post);
            }

            throw new Exception("Error creating post");
        }
    }


}
