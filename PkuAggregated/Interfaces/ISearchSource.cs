namespace PkuAggregated.Interfaces
{
    public interface ISearchSource
    {
        public Task<SearchResult> SearchAsync(string keyword);
    }

    public class SearchSourceInfo
    {
        required public string Name { get; set; }
        required public string Url { get; set; }
        required public string Id { get; set; }
    }

    public class SearchResult
    {
        required public List<SearchResultItem> Results { get; set; }
        required public SearchSourceInfo SourceInfo { get; set; }
        required public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class SearchResultItem
    {
        required public string Title { get; set; }
        required public string Description { get; set; }
        required public string Url { get; set; }
    }
}
