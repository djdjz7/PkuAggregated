using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PkuAggregated.Interfaces;
using PkuAggregated.Models;
using PkuAggregated.SearchSources;

namespace PkuAggregated.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<SearchResult[]> Search(
            [FromQuery] string keyword,
            [FromServices] IEnumerable<ISearchSource> searchSources
        )
        {
            return await Task.WhenAll(searchSources.Select(s => s.SearchAsync(keyword)));
        }

        [HttpGet]
        [Route("treehole")]
        public async Task<TreeholeSearchResult> SearchTreehole(
            [FromQuery] string keyword,
            [FromServices] Treehole treehole
        )
        {
            return await treehole.SearchAsync(keyword);
        }
    }
}
