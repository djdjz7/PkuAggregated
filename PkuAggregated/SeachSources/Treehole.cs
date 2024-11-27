using System.Diagnostics;
using System.Web;
using PkuAggregated.Interfaces;
using PkuAggregated.Models;

namespace PkuAggregated.SearchSources
{
    public class Treehole
    {
        public HttpClient HttpClient { get; set; } =
            new HttpClient() { BaseAddress = new Uri("https://treehole.pku.edu.cn/api/") };

        private SearchSourceInfo _searchSourceInfo = new SearchSourceInfo
        {
            Name = "树洞",
            Url = "https://treehole.pku.edu.cn",
            Id = "treehole",
        };

        public LoginStatus LoginStatus = LoginStatus.NotLoggedIn;

        public async Task<TreeholeSearchResult> SearchAsync(string keyword)
        {
            try
            {
                if (LoginStatus == LoginStatus.NotLoggedIn)
                {
                    await LoginAsync();
                }
                if (LoginStatus == LoginStatus.NeedSmsVerify)
                {
                    throw new Exception("NEED_SMS_VERIFY");
                }

                BeginSearch:
                var searchResponse = await HttpClient.GetFromJsonAsync<
                    TreeholeResponse<SearchResponseData>
                >($"pku_hole?page=1&limit=25&keyword={HttpUtility.UrlEncode(keyword)}");

                if (searchResponse is null)
                    throw new Exception("搜索失败。请求返回 null");

                if (searchResponse.code == 40001)
                {
                    LoginStatus = LoginStatus.NotLoggedIn;
                    await LoginAsync();
                    goto BeginSearch;
                }

                if (searchResponse.code == 40002)
                {
                    LoginStatus = LoginStatus.NeedSmsVerify;
                    throw new Exception("NEED_SMS_VERIFY");
                }

                if (searchResponse.code != 20000)
                    throw new Exception(
                        "搜索失败。" + Environment.NewLine + searchResponse.message
                    );

                var results = searchResponse.data?.data ?? [];
                var returnDataItems = results.Select(x => new TreeholeSearchResultItem
                {
                    Pid = x.pid,
                    Text = x.text,
                    Time = DateTime.UnixEpoch.AddSeconds(x.timestamp).ToLocalTime(),
                    ImageId = x.type == "image" ? x.pid : null,
                    CommentCount = x.reply,
                    StartCount = x.likenum,
                });

                return new TreeholeSearchResult
                {
                    IsSuccess = true,
                    Results = returnDataItems.ToList(),
                    SourceInfo = _searchSourceInfo,
                };
            }
            catch (Exception ex)
            {
                return new TreeholeSearchResult
                {
                    IsSuccess = false,
                    Results = [],
                    SourceInfo = _searchSourceInfo,
                    ErrorMessage = ex.Message,
                };
            }
        }

        private async Task LoginAsync()
        {
            var response = await HttpClient.PostAsJsonAsync(
                "login",
                new { uid = Params.Username, password = Params.Password }
            );

            var result = await response.Content.ReadFromJsonAsync<
                TreeholeResponse<LoginResponseData>
            >();
            if (result is null || !result.success)
                throw new Exception("登录失败。" + Environment.NewLine + result?.message);

            HttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result!.data.jwt);

            var testConnResponse = await HttpClient.GetFromJsonAsync<TreeholeResponse<object>>(
                "pku/tags"
            );
            if (testConnResponse is null)
                throw new Exception("未知错误。");

            if (testConnResponse.code == 40002)
            {
                LoginStatus = LoginStatus.NeedSmsVerify;
                throw new Exception("NEED_SMS_VERIFY");
            }
            LoginStatus = LoginStatus.LoggedIn;
        }
    }

    public enum LoginStatus
    {
        LoggedIn,
        NotLoggedIn,
        NeedSmsVerify,
    }
}
