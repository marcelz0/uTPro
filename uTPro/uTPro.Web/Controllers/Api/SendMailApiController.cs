using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.Controllers;
using uTPro.Common.Dictionaries;
using uTPro.Extension;
using uTPro.Extension.CurrentSite;
using uTPro.Foundation.FormContact;

namespace uTPro.Website.Backoffice.Controllers.Api
{
    [Route($"/api/SendMail")]
    public class SendMailApiController : UmbracoApiController
    {
        readonly IFormContactSendMail _formContact;
        readonly ICurrentSiteExtension _currentSite;
        readonly ILogger<SendMailApiController> _logger;

        public SendMailApiController(ILogger<SendMailApiController> logger, IFormContactSendMail formContact, ICurrentSiteExtension currentSite)
        {
            _formContact = formContact;
            _currentSite = currentSite;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post()
        {
            if (HttpContext.Request.HasFormContentType)
            {
                if (!_currentSite.RecaptchaV2Validate(HttpContext.Request.Form?["g-recaptcha-response"].ToString()))
                {
                    return Ok(_currentSite.GetDictionaryValue(Dictionaries.FormContact.ReponseMsg.Error));
                }

                var objLink = HttpContext.Request.Form?.FirstOrDefault(x => x.Key.StartsWith(Common.Constants.ConstFormContact.PrefixSysLinkPage));
                if (objLink != null && objLink.HasValue)
                {
                    string link = objLink.Value.Value;
                    var result = await _formContact.Reponse(HttpContext.Request);

                    if (HttpContext.Request.IsAjax())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Redirect(link);
                    }
                }
            }
            return Ok(_currentSite.GetDictionaryValue(Dictionaries.FormContact.ReponseMsg.Error));
        }


    }
}
