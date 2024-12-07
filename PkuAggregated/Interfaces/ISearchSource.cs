namespace PkuAggregated.Interfaces
{
    public interface ISearchSource
    {
        public Task<SearchResult> SearchAsync(string keyword);
    }

    public class SearchSourceInfo
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required string Id { get; set; }
    }

    public class SearchResult
    {
        public required List<SearchResultItem> Results { get; set; }
        public required SearchSourceInfo SourceInfo { get; set; }
        public required bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class SearchResultItem
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Url { get; set; }
    }
}
