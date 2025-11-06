using APIWeb.Dtos.Comments;
using APIWeb.Interfaces;
using APIWeb.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace APIWeb.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;
        public CommentController(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentsAsync()
        {
            var comments = await _commentsRepository.GetAllCommentsAsync();

            var selectedSTocks = comments.Select(s => s.ToCommentDto());
            return Ok(selectedSTocks);
        }

        [HttpGet("{id}", Name = "GetCommentById")]
        public async Task<IActionResult> GetCommentByIdAsync([FromRoute] int id)
        {
            var comment = await _commentsRepository.GetCommentByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommentAsync([FromBody] CreateCommentDto createCommentDto)
        {
            var commentModel = new Comment
            {
                Title = createCommentDto.Title,
                Content = createCommentDto.Content,
                CreatedOn = DateTime.UtcNow,
                StockId = createCommentDto.StockId
            };
            var createdComment = await _commentsRepository.CreateComment(commentModel);

            return CreatedAtAction(nameof(GetCommentByIdAsync), "Comment", new { id = createdComment.Id }, createdComment);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCommentAsync([FromRoute] int id)
        {
            await _commentsRepository.DeleteCommentAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int id, [FromBody] CreateCommentDto updateCommentDto)
        {
            var updatedComment = await _commentsRepository.UpdateCommentAsync(id, updateCommentDto);
            if (updatedComment == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
