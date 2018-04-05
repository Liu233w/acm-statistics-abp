using Microsoft.AspNetCore.Antiforgery;
using AcmStatisticsAbp.Controllers;

namespace AcmStatisticsAbp.Web.Host.Controllers
{
    public class AntiForgeryController : AcmStatisticsAbpControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
