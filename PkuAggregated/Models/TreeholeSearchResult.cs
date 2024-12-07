using PkuAggregated.Interfaces;

namespace PkuAggregated.Models
{
    public class TreeholeSearchResult
    {
        public required List<TreeholeSearchResultItem> Results { get; set; }
        public required SearchSourceInfo SourceInfo { get; set; }
        public required bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class TreeholeSearchResultItem
    {
        public required int Pid { get; set; }
        public required string Text { get; set; }
        public required DateTime Time { get; set; }
        public int? ImageId { get; set; }
        public int CommentCount { get; set; }
        public int StartCount { get; set; }
    }
}
