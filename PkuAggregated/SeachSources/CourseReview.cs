using System.Text.Json;
using PkuAggregated.Interfaces;
using PkuAggregated.Models;

namespace PkuAggregated.SearchSources
{
    public class CourseReview : ISearchSource
    {
        private CourseReviewResponse data;

        public CourseReview()
        {
            var content = File.ReadAllText("Data/CourseReviewData.json");
            data =
                JsonSerializer.Deserialize<CourseReviewResponse>(content)
                ?? throw new Exception("CourseReviewData.json parse failed.");
        }

        public Task<SearchResult> SearchAsync(string keyword)
        {
            return Task.FromResult(
                new SearchResult
                {
                    IsSuccess = true,
                    SourceInfo = new SearchSourceInfo
                    {
                        Name = "课程测评",
                        Url = "https://courses.pinzhixiaoyuan.com/",
                        Id = "course-review"
                    },
                    Results = data
                        .cDatas.Where(x =>
                            x.name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        )
                        .Select(x =>
                        {
                            return new SearchResultItem
                            {
                                Title = x.name,
                                Description = x.department,
                                Url = $"https://courses.pinzhixiaoyuan.com/courses/view/{x.id}"
                            };
                        })
                        .ToList()
                }
            );
        }
    }
}
