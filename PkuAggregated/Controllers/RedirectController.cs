using System.Diagnostics.Contracts;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PkuAggregated.SearchSources;

namespace PkuAggregated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        [HttpGet]
        [Route("portal")]
        public void RedirectPortalAppAsync(
            [FromServices] PortalApps portalApps,
            [FromQuery] string url
        )
        {
            Console.WriteLine(Request.GetEncodedPathAndQuery());
            var baseUrl = new Uri("https://portal.pku.edu.cn/portal2017/");
            var redirectUrl = new Uri(baseUrl, url);
            var cookies = portalApps.CookieContainer.GetCookies(
                new Uri("https://portal.pku.edu.cn/portal2017/")
            );
            foreach (Cookie cookie in cookies)
            {
                Console.WriteLine(cookie.ToString());
                Response.Cookies.Append(
                    cookie.Name,
                    cookie.Value,
                    new CookieOptions()
                    {
                        Domain = cookie.Domain,
                        Path = cookie.Path,
                        HttpOnly = cookie.HttpOnly,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                    }
                );
            }
            Response.Headers.Location = redirectUrl.ToString();
            Response.StatusCode = 302;
        }
    }
}
