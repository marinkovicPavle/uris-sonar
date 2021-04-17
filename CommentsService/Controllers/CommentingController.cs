using AutoMapper;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSocialBaz.Data;
using TheSocialBaz.Data.PostMock;
using TheSocialBaz.Model;
using TheSocialBaz.Model.Enteties;

namespace TheSocialBaz.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CommentingController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ICommentingRepository commentRepository;
        private readonly IPostMockRepository postRepository;
        private readonly IConfiguration configuration;
        private readonly Logger logger;

        public CommentingController(IPostMockRepository postRepository, IHttpContextAccessor contextAccessor, IMapper mapper, ICommentingRepository commentRepository, Logger logger, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
            this.configuration = configuration;
            this.logger = logger;
        }

        private bool Authorize(string key)
        {
            if (!key.StartsWith("Bearer"))
            {
                return false;
            }

            var keyOnly = key.Substring(key.IndexOf("Bearer") + 7);
            var username = configuration.GetValue<string>("Authorization:Username");
            var password = configuration.GetValue<string>("Authorization:Password");
            var base64EncodedBytes = System.Convert.FromBase64String(keyOnly);
            var user = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
 
            if ((username+":"+password) != user)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Return implemented options for API   
        /// </summary>
        /// <returns>Header key 'Allow' with allowed requests</returns>
        /// <remarks>
        /// Example of successful request \
        /// OPTIONS 'https://localhost:49877/api/comments' \
        /// </remarks>
        /// <response code="200">Return header key 'Allow' with allowed requests</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpOptions]
        public IActionResult GetCommentsOpstions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE");
            return Ok();
        }

        /// <summary>
        /// Return all comments    
        /// </summary>
        /// <param name="key">Authorization header</param>
        /// <returns>List of all created comments</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:49877/api/comments' \
        ///     --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw=='
        /// </remarks>
        /// <response code="200">Return list of comments</response>
        /// <response code="404">There is no comments</response>
        /// <response code="401">Authorization error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<List<Comment>> GetAllComments([FromHeader(Name = "Authorization")] string key)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }
            var comments = commentRepository.GetAllComments();
            if (comments == null || comments.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no comments.");
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = comments });
        }

        /// <summary>
        /// Return comment for provided comment ID    
        /// </summary>
        /// <returns>Comment with provided ID</returns>
        /// <remarks>
        /// Example of successful request \
        /// GET 'https://localhost:49877/api/comments/40b090d8-9e0f-470b-9e0e-2df13e05e935' \
        ///     --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw=='
        /// </remarks>
        /// <response code="200">Returns comment for provided ID</response>
        /// <response code="400">There is no comment with provided ID</response>
        /// <response code="401">Authorization error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{commentID}")]
        public ActionResult<Comment> GetCommentByID([FromHeader(Name = "Authorization")] string key, [FromRoute] Guid commentID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            var comment = commentRepository.GetCommentByID(commentID);

            if (comment == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "There is no comment with provided ID", content = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = comment });
        }

        /// <summary>
        /// Return comments from the specified post 
        /// </summary>
        /// <returns>List of comments for the specified post</returns>
        /// <remarks>
        /// GET 'https://localhost:49877/api/comments/byPostID' \
        ///     Example of successful request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'postID = 1' \
        ///         --param  'accountID = 1' \
        ///     Example of a failed request \
        ///         --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///         --param  'postID = 3' \
        ///         --param  'accountID = 4'
        /// </remarks>
        /// <param name="postID">Post ID</param>
        /// <param name="accountID">Account ID who is sending a request</param>
        /// <param name="key">Authorization header</param>
        /// <response code="200">Return list of comments for given post</response>
        /// <response code="400">Post with provided ID does not exist</response>
        /// <response code="401">Authorization error</response>
        /// <response code="404">There is no comment with provided post ID</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byPostID")]
        public ActionResult<List<Comment>> GetCommentsByPostID([FromHeader(Name = "Authorization")] string key, [FromQuery] int postID, [FromQuery] int accountID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (postRepository.GetPostByID(postID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Post with given ID does not exist", content = "" });
            }

            var comments = commentRepository.GetCommentsByPostID(postID, accountID);

            if (comments.Count == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "This post has no comments added", content = "" });
            }

            return StatusCode(StatusCodes.Status200OK, new { status = "OK", content = comments });
        }


        /// <summary>
        /// Creates new comment
        /// </summary>
        /// <param name="commentDto">Model of comment that is being added</param>
        /// <param name="accountID">Account ID that sends request</param>
        /// <param name="key">Authorization header</param>
        /// <returns></returns>
        /// <remarks>
        /// POST 'https://localhost:49877/api/comments/' \
        /// Example of successful request \
        ///  --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///  --param 'accountID = 2' \
        ///  --body \
        /// {     \
        ///     "postID": 1, \
        ///     "content": "New comment" \
        /// } \
        /// </remarks>
        /// <response code="201">Return confirmation that new comment is created</response>
        /// <response code="400">Post with given ID does not exist</response>
        /// <response code="401">Authorization error</response>
        /// <response code="500">Server Error while saving comment</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult CreateComment([FromHeader(Name = "Authorization")] string key, [FromBody] CommentingCreateDto commentDto, [FromQuery] int accountID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (postRepository.GetPostByID(commentDto.PostID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Post with given ID does not exist", content = "" });
            }

            var newComment = new Comment
            {
                CommentText = commentDto.CommentText,
                CommentDate = DateTime.Now,
                AccountID = accountID,
                PostID = commentDto.PostID
            };

            try
            {
                commentRepository.CreateComment(newComment);
                commentRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new comment with ID {0} in database", newComment.CommentID), null);

                return StatusCode(StatusCodes.Status201Created, new { status = "Comment is successfully created!", content = newComment });
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Comment with ID {0} not created, message: {1}", newComment.CommentID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Create error " + ex.Message, content = "" });
            }


        }

        /// <summary>
        /// Update comment with provied ID
        /// </summary>
        /// <param name="newComment">Comment model that is going to be updated</param>
        /// <param name="key">Authorization header</param>
        /// <returns></returns>
        /// <remarks>
        /// PUT 'https://localhost:49877/api/comments' \
        /// Example of successful request    \
        ///  --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw=='  \
        ///  --body \
        /// { \
        ///     "commentID": "1cc45ba4-bbb9-41ad-b8fa-c768a4f14ca5", \
        ///     "postID": 1, \
        ///     "content": "Updated succ!" \
        /// } 
        /// </remarks>
        /// <response code="200">Return confirmation that comment is updated</response>
        /// <response code="401">Authorization error</response>
        /// <response code="404">Comment with provied ID not found</response>
        /// <response code="500">Server Error while updating comment</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public IActionResult UpdateComment([FromHeader(Name = "Authorization")] string key, [FromBody] CommentingUpdateDto newComment)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            if (commentRepository.GetCommentByID(newComment.CommentID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "There is no comment with given ID!", content = "" });
            }

            var commentToUpdate = commentRepository.GetCommentByID(newComment.CommentID);

            if (commentToUpdate.PostID != newComment.PostID)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = "Post ID can not be changed!", content = "" });
            }

            commentToUpdate.CommentText = newComment.CommentText;

            try
            {
                commentRepository.UpdateComment(commentToUpdate);
                commentRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated comment with ID {0} in database", commentToUpdate.CommentID), null);

                return StatusCode(StatusCodes.Status200OK, new { status = "Comment updated!", content = commentToUpdate });

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Comment with ID {0} not updated, message: {1}", commentToUpdate.CommentID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Update error", content = "" });


            }

        }

        /// <summary>
        /// Delete comment with provied ID
        /// </summary>
        /// <param name="commentID">Comment ID that is going to be removed</param>
        /// <param name="key">Authorization header</param>
        /// <remarks>        
        /// Example of successful request    \
        /// DELETE 'https://localhost:49877/api/comments' \
        ///     --header 'Authorization: Bearer YWRtaW46c3VwZXJBZG1pbjEyMw==' \
        ///     --param  'commentID = 40b090d8-9e0f-470b-9e0e-2df13e05e935'
        /// </remarks>
        /// <response code="200">Comment successfully deleted</response>
        /// <response code="401">Authorization error</response>
        /// <response code="404">Comment with provided ID does not exist</response>
        /// <response code="500">Server Error while deleting comment</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public IActionResult DeleteComment([FromHeader(Name = "Authorization")] string key, [FromQuery] Guid commentID)
        {
            if (!Authorize(key))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = "Authorization failed",
                    content = ""
                });
            }

            var comment = commentRepository.GetCommentByID(commentID);
            if (comment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { status = "Comment with provided id not found!", content = "" });
            }
            try
            {
                commentRepository.DeleteComment(commentID);
                commentRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully deleted comment with ID {0} from database", commentID), null);

                return StatusCode(StatusCodes.Status200OK, new { status = "Comment successfully deleted", content = "" });
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting comment with ID {0}, message: {1}", commentID, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "Delete error", content = "" });
            }

        }
    }
}
