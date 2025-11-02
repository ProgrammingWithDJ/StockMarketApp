using APIWeb.Data;
using APIWeb.Dtos.Comments;
using APIWeb.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIWeb.Repositories
{
    public class CommentsRepository :ICommentsRepository
    {
        private readonly ApplicationDBContext _dBContext;
        public CommentsRepository(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
        }


        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _dBContext.Comment.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            var comment = await _dBContext.Comment.FindAsync(id);
            if(comment == null)
            {
                return null;
            }
            return comment;
        }

        public async Task<Comment> CreateCommentDto(Comment comment)
        {
            _dBContext.Comment.Add(comment);
            await _dBContext.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _dBContext.Comment.FindAsync(id);
            if(comment != null)
            {
                _dBContext.Comment.Remove(comment);
                await _dBContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _dBContext.Comment.FindAsync(id);
            if (comment != null)
            {
                _dBContext.Comment.Remove(comment);
                await _dBContext.SaveChangesAsync();
            }
        }

        public async Task<Comment>? UpdateCommentAsync(int id, CreateCommentDto updateCommentDto)
        {
            var existingComment = await _dBContext.Comment.FindAsync(id);
            if (existingComment == null)
            {
                return null;
            }
            existingComment.Title = updateCommentDto.Title;
            existingComment.Content = updateCommentDto.Content;

            _dBContext.Comment.Update(existingComment);
            await _dBContext.SaveChangesAsync();
            return existingComment;
        }
    }
}
