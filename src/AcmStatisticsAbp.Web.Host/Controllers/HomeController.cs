// <copyright file="HomeController.cs" company="西北工业大学ACM开发组">
// Copyright (c) 西北工业大学ACM开发组. All rights reserved.
// </copyright>

namespace AcmStatisticsAbp.Web.Host.Controllers
{
    using System.Threading.Tasks;
    using Abp;
    using Abp.Extensions;
    using Abp.Notifications;
    using Abp.Timing;
    using AcmStatisticsAbp.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AcmStatisticsAbpControllerBase
    {
        private readonly INotificationPublisher notificationPublisher;

        public HomeController(INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;
        }

        public IActionResult Index()
        {
            return this.Redirect("/swagger");
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await this.notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin });

            return this.Content("Sent notification: " + message);
        }
    }
}
