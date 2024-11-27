using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Web;
using PkuAggregated.Interfaces;

namespace PkuAggregated.SearchSources
{
    public class PortalApps : ISearchSource
    {
        public CookieContainer CookieContainer;
        private HttpClientHandler _handler;
        private HttpClient _httpClient;

        public bool IsLoggedIn = false;

        public IHttpContextAccessor HttpContext { get; set; }
        private SearchSourceInfo _sourceInfo = new SearchSourceInfo
        {
            Name = "门户应用",
            Url = "https://portal.pku.edu.cn/",
            Id = "portal-apps",
        };

        public PortalApps(IHttpContextAccessor httpContext)
        {
            CookieContainer = new CookieContainer();
            _handler = new HttpClientHandler() { CookieContainer = CookieContainer };
            _httpClient = new HttpClient(_handler);
            HttpContext = httpContext;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var response = await _httpClient.PostAsync(
                "https://iaaa.pku.edu.cn/iaaa/oauthlogin.do",
                new FormUrlEncodedContent(
                    [
                        KeyValuePair.Create("appid", "portal2017"),
                        KeyValuePair.Create("userName", username),
                        KeyValuePair.Create("password", password),
                        KeyValuePair.Create(
                            "redirUrl",
                            "https://portal.pku.edu.cn/portal2017/ssoLogin.do"
                        )
                    ]
                )
            );

            var result =
                await response.Content.ReadFromJsonAsync<LoginResult>()
                ?? throw new Exception("解析响应时异常。");
            if (!result.success)
                throw new Exception(result.errors?.msg ?? "未知错误。");
            if (string.IsNullOrEmpty(result.token))
                throw new Exception("token 为空。");

            return result.token;
        }

        public async Task SsoLoginAsync(string token)
        {
            var response = await _httpClient.GetAsync("https://portal.pku.edu.cn/portal2017");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }

        public async Task<SearchResult> SearchAsync(string keyword)
        {
            try
            {
                var request = HttpContext.HttpContext?.Request;
                if (request is null)
                    throw new Exception("未能获取请求信息。");
                var host = request.Host;
                var scheme = request.Scheme;
                if (!IsLoggedIn)
                {
                    var token = await LoginAsync(Params.Username, Params.Password);
                    await SsoLoginAsync(token);
                    IsLoggedIn = true;
                }
                var response = await _httpClient.GetAsync(
                    $"https://portal.pku.edu.cn/portal2017/account/searchBiz.do?keyword={HttpUtility.UrlEncode(keyword)}"
                );

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    IsLoggedIn = false;
                    Console.WriteLine("No Auth!");
                }

                Console.WriteLine(await response.Content.ReadAsStringAsync());

                var body =
                    await response.Content.ReadFromJsonAsync<PortalAppSearchResult>()
                    ?? throw new Exception("响应体解析为空。");

                var data = body.rows?.Select(x => new SearchResultItem
                {
                    Title = x.portletName,
                    Description = x.description,
                    Url =
                        $"{scheme}://{host}/api/redirect/portal?url={HttpUtility.UrlEncode(x.portletHref)}",
                });

                return new SearchResult
                {
                    IsSuccess = true,
                    Results = data?.ToList() ?? [],
                    SourceInfo = _sourceInfo
                };
            }
            catch (Exception e)
            {
                return new SearchResult
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                    SourceInfo = _sourceInfo,
                    Results = []
                };
            }
        }
    }

    public class LoginResult
    {
        public bool success { get; set; }
        public string? token { get; set; }
        public Error? errors { get; set; }
    }
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public class PortalAppSearchResult
    {
        public bool success { get; set; }
        public Row[] rows { get; set; }
    }

    public class Row
    {
        public string description { get; set; }
        public string favoriteId { get; set; }
        public int hitTimes { get; set; }
        public string imageUrl { get; set; }
        public string isMenu { get; set; }
        public string portletHref { get; set; }
        public string portletId { get; set; }
        public string portletImageId { get; set; }
        public string portletName { get; set; }
        public string portletNameEn { get; set; }
        public string portletOrder { get; set; }
        public string portletPos { get; set; }
        public string target { get; set; }
        public string userId { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string msg { get; set; }
    }
}
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
