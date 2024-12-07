using System.Security.Cryptography;
using System.Text;
using System.Web;
using AngleSharp.Parser.Html;
using PkuAggregated.Interfaces;

namespace PkuAggregated.SeachSources;

public class Bbs : ISearchSource
{
    private bool _loggedIn = false;
    private SearchSourceInfo _sourceInfo = new SearchSourceInfo()
    {
        Name = "未名 BBS",
        Url = "https://bbs.pku.edu.cn/",
        Id = "bbs",
    };
    private HttpClient _httpClient = new() { BaseAddress = new Uri("https://bbs.pku.edu.cn/") };

    public async Task<SearchResult> SearchAsync(string keyword)
    {
        try
        {
            if (!_loggedIn)
            {
                await LoginAsync(Params.BbsUsername, Params.BbsPassword);
                _loggedIn = true;
            }
            var repsonse = await _httpClient.GetAsync(
                $"/v2/search.php?mode=post&timeorder=1&key={HttpUtility.UrlEncode(keyword)}"
            );
            var html = await repsonse.Content.ReadAsStringAsync();
            HtmlParser htmlParser = new HtmlParser();
            var document = await htmlParser.ParseAsync(html);
            var blocks = document.QuerySelectorAll(
                "#page-search > .search-result > .block.post-block"
            );
            var results = new List<SearchResultItem>();
            foreach (var block in blocks)
            {
                var link = block.QuerySelector("a.block-link").Attributes["href"].Value.Trim();
                var fullLink = new Uri(new Uri("https://bbs.pku.edu.cn/v2/"), link).ToString();
                var title = block.QuerySelector(".title").TextContent.Trim();
                var from = block.QuerySelector(".from").TextContent.Trim();
                var briefList = block
                    .QuerySelectorAll(".brief a")
                    .Select(x => x.TextContent.Trim());
                var brief = string.Join(Environment.NewLine, briefList);
                var name = block.QuerySelector(".name").TextContent.Trim();
                results.Add(
                    new SearchResultItem()
                    {
                        Title = title,
                        Description = brief,
                        Url = fullLink,
                    }
                );
            }
            return new SearchResult()
            {
                IsSuccess = true,
                SourceInfo = _sourceInfo,
                Results = results,
            };
        }
        catch (Exception ex)
        {
            return new SearchResult()
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
                SourceInfo = _sourceInfo,
                Results = [],
            };
        }
    }

    public async Task LoginAsync(string username, string password)
    {
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        var validateStr = $"{password}{username}{timestamp}{password}";
        var validateBytes = MD5.HashData(Encoding.ASCII.GetBytes(validateStr));
        var sb = new StringBuilder();
        foreach (var b in validateBytes)
            sb.Append(b.ToString("x2"));
        var validate = sb.ToString();
        var response = await _httpClient.PostAsync(
            "https://bbs.pku.edu.cn/v2/ajax/login.php",
            new FormUrlEncodedContent(
                [
                    KeyValuePair.Create("username", username),
                    KeyValuePair.Create("password", password),
                    KeyValuePair.Create("keepalive", "0"),
                    KeyValuePair.Create("time", timestamp.ToString()),
                    KeyValuePair.Create("t", validate),
                ]
            )
        );
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (result is null)
            throw new Exception("未能完成登录，响应解析为 null");
        if (!result.success)
            throw new Exception("登录失败，请检查用户名和密码");
    }
}

public class LoginResponse
{
    public bool success { get; set; }
}
