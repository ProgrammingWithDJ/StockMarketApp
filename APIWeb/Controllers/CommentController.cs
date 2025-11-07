using APIWeb.Dtos.Comments;
using APIWeb.Interfaces;
using APIWeb.Mappers;
using APIWeb.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APIWeb.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentsRepository commentsRepository, IStockRepository stockRepository)
        {
            _commentsRepository = commentsRepository;
            _stockRepository = stockRepository;
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

        [HttpPost("{stockId}")]
        public async Task<IActionResult> CreateCommentAsync([FromRoute] int stockId,[FromBody] CreateCommentDto createCommentDto)
        {
            var stock = await _stockRepository.StockExists(stockId);

            if(!stock)
            {
                return BadRequest($"Stock with ID {stockId} not found.");
            }

            var commentModel = CommentMappers.ToCommentFromCreateDto(createCommentDto, stockId);
           
            
            var createdComment = await _commentsRepository.CreateComment(commentModel);

            return NoContent();
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
        public async Task<IActionResult> UpdateCommentAsync([FromRoute] int id, [FromBody] UpdateCommentDto updateCommentDto)
        {
            var comment = await _commentsRepository.CommentExists(id);

            if (!comment)
            {
                return BadRequest($"Comment with ID {comment} not found.");
            }

            var updatedComment = await _commentsRepository.UpdateCommentAsync(id, updateCommentDto.ToCommentFromUpdate());
            if (updatedComment == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
