using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using PkuAggregated.Models;
using PkuAggregated.SearchSources;

namespace PkuAggregated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeholeController : ControllerBase
    {
        [HttpPost]
        [Route("verify")]
        public async Task<bool> VerifyTreehole(
            [FromServices] Treehole treehole,
            [FromBody] string code
        )
        {
            var response = await treehole.HttpClient.PostAsJsonAsync(
                "jwt_msg_verify",
                new { valid_code = code }
            );
            var result = await response.Content.ReadFromJsonAsync<TreeholeResponse<object>>();
            return result?.success ?? false;
        }

        [HttpPost]
        [Route("send-msg")]
        public async Task<bool> SendMessageTreehole([FromServices] Treehole treehole)
        {
            var response = await treehole.HttpClient.PostAsync("jwt_send_msg", null);
            var result = await response.Content.ReadFromJsonAsync<TreeholeResponse<object>>();
            if (result?.success ?? false)
            {
                treehole.LoginStatus = LoginStatus.LoggedIn;
                return true;
            }
            else
                return false;
        }

        [HttpGet]
        [Route("details/{pid}")]
        public async Task<CommentData[]> GetTreeholeDetails(
            [FromRoute] string pid, [FromServices] Treehole treehole
        )
        {
            var response = await treehole.HttpClient.GetFromJsonAsync<TreeholeResponse<DetailsResponseData>>($"pku_comment_v3/{pid}?page=1");
            if (response is null)
                throw new Exception("Failed to fetch, response is null.");
            if (!response.success)
                throw new Exception("Unable to fetch comment:\n" + response.message);
            if (response?.data is null)
                throw new Exception("Unknown error: data is null");
            var tempList = new List<CommentData>(response.data.total);
            tempList.AddRange(response.data.data ?? []);
            while (response.data?.next_page_url != null)
            {
                response = await treehole.HttpClient.GetFromJsonAsync<TreeholeResponse<DetailsResponseData>>(response.data.next_page_url.Replace("http://treehole.pku.edu.cn/api/", ""));
                if (response is null)
                    throw new Exception("Failed to fetch, response is null.");
                if (!response.success)
                    throw new Exception("Unable to fetch comment:\n" + response.message);
                if (response?.data is null)
                    throw new Exception("Unknown error: data is null");
                tempList.AddRange(response.data.data);
            }
            return tempList.ToArray();
        }

        [HttpGet]
        [Route("image/{id}")]
        public async Task GetImage([FromRoute] string id, [FromServices] Treehole treehole)
        {
            using var stream = await treehole.HttpClient.GetStreamAsync(
                $"https://treehole.pku.edu.cn/api/pku_image/{id}"
            );
            await stream.CopyToAsync(Response.Body);
        }
    }
}
