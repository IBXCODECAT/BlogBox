using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistance;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Net;

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
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.Created)]
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
                return new ObjectResult(post) { StatusCode = StatusCodes.Status201Created };
            }

            throw new Exception("Error creating post");
        }

        [HttpPut("{id}", Name = "Update")]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public ActionResult<Post> Update([FromBody]Post request)
        {
            Post? postToUpdate = context.posts.Find(request.Id);

            if(postToUpdate is null)
            {
                Post newPost = new Post()
                {
                    Id = request.Id,
                    Title = request.Title,
                    Body = request.Body,
                    Date = request.Date
                };

                context.posts.Add(newPost);

                bool success = context.SaveChanges() > 0;

                return success ? new ObjectResult(newPost) { StatusCode = StatusCodes.Status201Created } : throw new Exception("Error creating or updating resource.");
            }
            else
            {
                postToUpdate.Title = request.Title;
                postToUpdate.Body = request.Body;
                postToUpdate.Date = request.Date;

                bool success = context.SaveChanges() > 0;

                return success ? new ObjectResult(postToUpdate) { StatusCode = StatusCodes.Status200OK } : throw new Exception("Error creating or updating resource.");
            }
        }
    }


}
