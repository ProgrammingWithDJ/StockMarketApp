namespace APIWeb.Dtos.Comments
{
    public class CreateCommentDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? StockId { get; set; } // Nullable
    }

}
