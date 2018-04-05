namespace AcmStatisticsAbp.Web.Host.Controllers
{
    using AcmStatisticsAbp.Controllers;
    using Microsoft.AspNetCore.Antiforgery;

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
