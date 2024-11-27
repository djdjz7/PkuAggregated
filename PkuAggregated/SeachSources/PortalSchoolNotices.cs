using System.Web;
using Microsoft.VisualBasic;
using PkuAggregated.Interfaces;
using PkuAggregated.Models;

namespace PkuAggregated.SearchSources
{
    public class PortalSchoolNotices : ISearchSource
    {
        private HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://portal.pku.edu.cn/portal2017/notice/")
        };

        public async Task<SearchResult> SearchAsync(string keyword)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<NoticeResponse>(
                    $"retrAllSchoolNotice.do?keyword={HttpUtility.UrlDecode(keyword)}&limit=20&start=0"
                );

                if (response is null || !response.success)
                    throw new Exception("未知错误。");

                return new SearchResult()
                {
                    IsSuccess = true,
                    Results = response
                        .rows.Select(x => new SearchResultItem
                        {
                            Title = x.Title,
                            Url =
                                $"https://portal.pku.edu.cn/portal2017/#/schoolNoticeDetail/{x.Number}",
                            Description = $"{x.Department} {x.Time}"
                        })
                        .ToList(),
                    SourceInfo = new SearchSourceInfo
                    {
                        Name = "学校公告",
                        Url = "https://portal.pku.edu.cn/portal2017/#/deptNotices/1/ALL",
                        Id = "portal-school-notices",
                    }
                };
            }
            catch (Exception ex)
            {
                return new SearchResult()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Results = [],
                    SourceInfo = new SearchSourceInfo
                    {
                        Name = "学校公告",
                        Url = "https://portal.pku.edu.cn/portal2017/#/deptNotices/1/ALL",
                        Id = "portal-school-notices",
                    }
                };
            }
        }
    }
}
