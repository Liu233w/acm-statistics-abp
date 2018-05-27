// <copyright file="HomeController.cs" company="西北工业大学ACM技术组">
// Copyright (c) 西北工业大学ACM技术组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Web.Host.Controllers
{
    using Abp.Notifications;
    using AcmStatisticsAbp.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AcmStatisticsAbpControllerBase
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly INotificationPublisher notificationPublisher;

        public HomeController(INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;
        }

        public IActionResult Index()
        {
            return this.Redirect("/swagger");
        }
    }
}
