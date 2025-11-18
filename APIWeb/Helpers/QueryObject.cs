namespace APIWeb.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } =null;

        public string? CompanyName { get; set; } = null;

        public string orderBy { get; set; } = null;

        public int pageNumber { get; set; } = 1;

        public int pageSize { get; set; } = 10;
    }
}
