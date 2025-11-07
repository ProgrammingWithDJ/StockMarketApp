using APIWeb.Dtos.Comments;

namespace APIWeb.Interfaces
{
    public interface ICommentsRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();

        Task<Comment>? GetCommentByIdAsync(int id);

        Task<Comment> CreateComment(Comment comment);

        Task DeleteCommentAsync(int id);

        Task<bool> CommentExists(int id);

        Task<Comment>? UpdateCommentAsync(int id, Comment commentModel);
    }
}
