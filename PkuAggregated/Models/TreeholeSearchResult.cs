using PkuAggregated.Interfaces;

namespace PkuAggregated.Models
{
    public class TreeholeSearchResult
    {
        required public List<TreeholeSearchResultItem> Results { get; set; }
        required public SearchSourceInfo SourceInfo { get; set; }
        required public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class TreeholeSearchResultItem
    {
        required public int Pid { get; set; }
        required public string Text { get; set; }
        required public DateTime Time { get; set; }
        public int? ImageId { get; set; }
        public int CommentCount { get; set; }
        public int StartCount { get; set; }
    }
}
