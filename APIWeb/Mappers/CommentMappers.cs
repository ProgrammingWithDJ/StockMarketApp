using APIWeb.Dtos.Comments;
using APIWeb.Dtos.Stocks;

namespace APIWeb.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };
        }

        public static Comment ToCommentFromCreateDto(this CreateCommentDto createCommentDto)
        {
           return new Comment
           {
               Title = createCommentDto.Title,
               Content = createCommentDto.Content,
               StockId = createCommentDto.StockId,
               CreatedOn = DateTime.UtcNow
           };
        }
    }
}
