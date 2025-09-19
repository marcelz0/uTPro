using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;
using uTPro.Common.Dictionaries;
using uTPro.Common.Models.Feature.Search;
using uTPro.Extension.CurrentSite;
using uTPro.Extension.Search;

namespace uTPro.Website.Backoffice.Controllers.Api
{
    [Route($"/api/Search")]
    public class SearchApiController : UmbracoApiController
    {
        readonly ISearchExtension _searchExtension;
        readonly ICurrentSiteExtension _currentSite;
        public SearchApiController(ISearchExtension searchExtension, ICurrentSiteExtension currentSite)
        {
            _searchExtension = searchExtension;
            _currentSite = currentSite;
        }

        [HttpPost($"/api/Search")]
        public IActionResult Post([FromForm] SearchRequest item)
        {
            if (HttpContext.Request.HasFormContentType)
            {
                return Ok(_searchExtension.GetContentJson(item));
            }
            return Ok(_currentSite.GetDictionaryValue(Dictionaries.FormContact.ReponseMsg.Error));
        }

        [HttpPost($"/api/Search/Media")]
        public IActionResult PostMedia([FromForm] SearchRequest item)
        {
            if (HttpContext.Request.HasFormContentType)
            {
                return Ok(_searchExtension.GetMediaJson(item));
            }
            return Ok(_currentSite.GetDictionaryValue(Dictionaries.FormContact.ReponseMsg.Error));
        }
    }
}
